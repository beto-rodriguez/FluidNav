﻿using FluidNav.Flowing;

namespace FluidNav;

/// <summary>
/// Defines a fluid container.
/// </summary>
/// <remarks>
/// Initializes a new instance of the <see cref="FlowNavigation"/> class.
/// </remarks>
/// <param name="provider">The service provider.</param>
/// <param name="view">The view.</param>
/// <param name="map">The map.</param>
public class FlowNavigation(IServiceProvider provider, IFluidHost view, RouteMap map)
{
    private readonly IServiceProvider _services = provider;
    private List<string> _navigationStack = [];
    private int _activeIndex;
    private Type? _activeViewType;
    private FluidView? _activeView;

    /// <summary>
    /// Gets the main fluid page.
    /// </summary>
    public static FlowNavigation Current { get; internal set; } = null!;

    /// <summary>
    /// Gets the active routes.
    /// </summary>
    public RouteMap ActiveRoutes { get; } = map;

    /// <summary>
    /// Indicates whether the views fade when navigating.
    /// </summary>
    public bool Fades { get; set; } = false;

    /// <summary>
    /// Gets or sets the transition duration.
    /// </summary>
    public uint TransitionDuration { get; set; } = 500;

    /// <summary>
    /// Gets the view host.
    /// </summary>
    public IFluidHost View { get; set; } = view;

    /// <summary>
    /// Called when navigating.
    /// </summary>
    public event Action<View> Navigating;

    public void Initialize(IFluidHost host)
    {
        Current.View = host;

        _ = Current.GoTo(
            Current.ActiveRoutes.DefaultRoute ?? throw new Exception("No default route was found."));
    }

    /// <summary>
    /// Gets the view of the given type.
    /// </summary>
    /// <typeparam name="TView">The type of the view.</typeparam>
    /// <returns>The view</returns>
    public TView GetView<TView>() where TView : View =>
        (TView)GetView(typeof(TView).Name);

    /// <summary>
    /// Gets the view of the given route name.
    /// </summary>
    /// <param name="routeName">The route name.</param>
    /// <returns>The view</returns>
    public View GetView(string routeName)
    {
        var targetType = ActiveRoutes[routeName];

        return (View?)_services.GetService(targetType) ??
            throw new Exception($"View {targetType.Name} not found");
    }

    /// <summary>
    /// Navigates to the specified type.
    /// </summary>
    /// <typeparam name="TRoute">The type of the route.</typeparam>
    /// <param name="queryParams">The query parameters, e.g. id=10&name=john.</param>
    public Task<View> GoTo<TRoute>(string? queryParams = null) =>
        GoTo(typeof(TRoute).Name + (queryParams is null ? string.Empty : $"?{queryParams}"));

    /// <summary>
    /// Navigates to the specified view type.
    /// </summary>
    /// <param name="type">The type of the route.</param>
    /// <param name="queryParams">The query parameters, e.g. id=10&name=john.</param>
    public Task<View> GoTo(Type type, string? queryParams = null) =>
        GoTo(type.Name + (queryParams is null ? string.Empty : $"?{queryParams}"));

    /// <summary>
    /// Navigates to the specified view type.
    /// </summary>
    /// <param name="route">The route.</param>
    public Task<View> GoTo(string route)
    {
        var routeName = route.Split('?')[0];

        if (!ActiveRoutes.Contains(routeName)) throw new Exception($"Route {routeName} not found");

        _navigationStack = _navigationStack.Take(_activeIndex + 1).ToList();
        _navigationStack.Add(route);
        _activeIndex = _navigationStack.Count - 1;

        return Go();
    }

    /// <summary>
    /// Navigates back.
    /// </summary>
    public Task<View> GoBack()
    {
        _activeIndex--;
        if (_activeIndex < 0) _activeIndex = 0;

        return Go();
    }

    /// <summary>
    /// Navigates forward.
    /// </summary>
    public Task<View> GoNext()
    {
        _activeIndex++;
        if (_activeIndex >= _navigationStack.Count) _activeIndex = _navigationStack.Count - 1;

        return Go();
    }

    /// <summary>
    /// Refreshes the current view.
    /// </summary>
    public void OnHotReloaded() => _ = Go(true);

    private bool _isNavigating;

    private async Task<View> Go(bool isHotReload = false)
    {
        if (_isNavigating) return _activeView ?? throw new Exception("No active view found.");

        var routeParts = _navigationStack[_activeIndex].Split('?');

        var routeName = routeParts[0];
        var queryParams = routeParts.Length > 1 ? routeParts[1] : string.Empty;

        var routeParams = (RouteParams?)_services.GetService(typeof(RouteParams));
        routeParams?.Clear();
        if (queryParams.Length > 0) routeParams?.SetParams(queryParams);

        var targetType = ActiveRoutes[routeName];

        var nextView = (View?)_services.GetService(targetType) ??
            throw new Exception($"View {targetType.Name} not found");

        if (!isHotReload && nextView == _activeView) return _activeView;

        if (isHotReload)
        {
            if (nextView is FluidView fv)
            {
                fv.Content = fv.GetView();
                fv.OnBreakpointChanged();
                _ = fv.TransitionView?.Complete(targetType);
            }

            return nextView;
        }

        _isNavigating = true;
        Navigating?.Invoke(nextView);

        nextView.IsVisible = true;

        // if we don't call the animations api it seems that the view is not visible for a reason.
        //nextView.Opacity = 1;
        if (Fades) _ = nextView.FadeTo(1, 1);

        if (_activeView is not null)
        {
            _activeView.Leaving();

            if (_activeView.TransitionView?.TransitionBounds is not null)
            {
                var tb = _activeView.TransitionView.TransitionBounds;

                _ = _activeView
                    .Flows(
                        (VisualElement.TranslationXProperty, tb.Left),
                        (VisualElement.TranslationYProperty, tb.Top),
                        (VisualElement.WidthRequestProperty, tb.Width),
                        (VisualElement.HeightRequestProperty, tb.Height))
                    .Animate(duration: TransitionDuration);

                _ = await _activeView.TransitionView.Animate(targetType);
            }
        }

        if (Fades) _ = (_activeView?.FadeTo(0, TransitionDuration));
        Current?.View.ShowView(nextView);

        if (nextView is FluidView fluidView)
        {
            fluidView.Entering();
            fluidView.OnBreakpointChanged();

            if (fluidView.TransitionView is not null)
            {
                var tb = fluidView.TransitionView.TransitionBounds;

                nextView.WidthRequest = tb.Width;
                nextView.HeightRequest = tb.Height;
                nextView.TranslationX = tb.X;
                nextView.TranslationY = tb.Y;

                var flowView = (Page?)Current?.View ?? throw new Exception("unable to get current view.");

                _ = nextView
                    .Flows(
                        (VisualElement.TranslationXProperty, 0),
                        (VisualElement.TranslationYProperty, 0),
                        (VisualElement.WidthRequestProperty, flowView.Width),
                        (VisualElement.HeightRequestProperty, flowView.Height))
                    .Animate(duration: TransitionDuration);

                if (_activeViewType is not null)
                {
                    _ = fluidView.TransitionView.Complete(_activeViewType);
                    _ = await fluidView.TransitionView.Animate(targetType);
                }
            }

            _activeViewType = targetType;
            if (_activeView is not null) _activeView.IsVisible = false;
            _activeView = fluidView;
        }

        _isNavigating = false;

        return nextView;
    }
}
