using FluidNav.Flowing;

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
    /// Gets the view host.
    /// </summary>
    public IFluidHost View { get; set; } = view;

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
    public TView GetView<TView>() where TView : View
    {
        return (TView)GetView(typeof(TView).Name);
    }

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
    public Task<View> GoTo<TRoute>(string? queryParams = null)
    {
        return GoTo(typeof(TRoute).Name + (queryParams is null ? string.Empty : $"?{queryParams}"));
    }

    /// <summary>
    /// Navigates to the specified view type.
    /// </summary>
    /// <param name="type">The type of the route.</param>
    /// <param name="queryParams">The query parameters, e.g. id=10&name=john.</param>
    public Task<View> GoTo(Type type, string? queryParams = null)
    {
        return GoTo(type.Name + (queryParams is null ? string.Empty : $"?{queryParams}"));
    }

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
    public void OnHotReloaded()
    {
        _ = Go(true);
    }

    private async Task<View> Go(bool isHotReload = false)
    {
        var routeParts = _navigationStack[_activeIndex].Split('?');

        var routeName = routeParts[0];
        var queryParams = routeParts.Length > 1 ? routeParts[1] : string.Empty;

        var routeParams = (RouteParams?)_services.GetService(typeof(RouteParams));
        routeParams?.Clear();
        if (queryParams.Length > 0) routeParams?.SetParams(queryParams);

        var targetType = ActiveRoutes[routeName];

        var nextView = (View?)_services.GetService(targetType) ??
            throw new Exception($"View {targetType.Name} not found");

        if (isHotReload)
        {
            if (nextView is FluidView fv)
            {
                fv.Content = fv.GetView();
                _ = fv.TransitionView?.Complete(targetType);
            }

            return nextView;
        }

        nextView.IsVisible = true;
        // if we don't call the animations api it seems that the view is not visible for a reason.
        //nextView.Opacity = 1;
        _ = nextView.FadeTo(1, 1);

        if (_activeView is not null)
        {
            _activeView.OnLeaving();

            if (_activeView.TransitionView?.TransitionBounds is not null)
            {
                var tb = _activeView.TransitionView.TransitionBounds;

                _ = _activeView.Flows(
                    (VisualElement.TranslationXProperty, tb.Left),
                    (VisualElement.TranslationYProperty, tb.Top),
                    (VisualElement.WidthRequestProperty, tb.Width),
                    (VisualElement.HeightRequestProperty, tb.Height))
                    .Animate();

                _ = await _activeView.TransitionView.Animate(targetType);
            }
        }

        _ = (_activeView?.FadeTo(0, 500));
        Current?.View.ShowView(nextView);

        if (nextView is FluidView fluidView)
        {
            fluidView.OnEntering();

            if (fluidView.TransitionView is not null)
            {
                var tb = fluidView.TransitionView.TransitionBounds;

                nextView.WidthRequest = tb.Width;
                nextView.HeightRequest = tb.Height;
                nextView.TranslationX = tb.X;
                nextView.TranslationY = tb.Y;

                var flowView = (Page?)Current?.View ?? throw new Exception("unable to get current view.");

                _ = nextView.Flows(
                    (VisualElement.TranslationXProperty, 0),
                    (VisualElement.TranslationYProperty, 0),
                    (VisualElement.WidthRequestProperty, flowView.Width),
                    (VisualElement.HeightRequestProperty, flowView.Height))
                    .Animate();

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

        return nextView;
    }
}
