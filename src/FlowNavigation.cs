using FluidNav.Flowing;

namespace FluidNav;

/// <summary>
/// Defines a fluid container.
/// </summary>
/// <remarks>
/// Initializes a new instance of the <see cref="FlowNavigation"/> class.
/// </remarks>
public class FlowNavigation
{
    private bool _isInitialized = false;
    private View? _currentView;
    private int _zIndex = 0;
    private readonly IServiceProvider _services;
    private List<string> _navigationStack = [];
    private int _activeIndex;
    private Type? _activeViewType;
    internal FluidView _enteringView = null!;
    private FluidView? _activeView;
    private bool _isNavigating;
    private static FlowNavigation s_current = null!;
    private static readonly Dictionary<int, int> s_screens = new()
    {
        { (int)BreakPoint.sm, 640 },
        { (int)BreakPoint.md, 768 },
        { (int)BreakPoint.lg, 1024 },
        { (int)BreakPoint.xl, 1280 },
        { (int)BreakPoint.xxl, 1536 }
    };

    public FlowNavigation(IServiceProvider serviceProvider, IFluidHost view, RouteMap map)
    {
        _services = serviceProvider;
        View = view;
        ActiveRoutes = map;

        ((ContentPage)view).SizeChanged += (_, _) =>
        {
            if (!_isInitialized)
            {
                _isInitialized = true;
                ActiveBreakpoint = GetBreakpoint();

                _ = Current.GoTo(Current.ActiveRoutes.DefaultRoute
                    ?? throw new Exception("No default route was found."));
            }

            if (_currentView is not null)
            {
                _currentView.WidthRequest = Current.View.Width;
                _currentView.HeightRequest = Current.View.Height;
            }

            var bp = GetBreakpoint();
            if (bp == ActiveBreakpoint) return;

            ActiveBreakpoint = bp;
            BreakpointChanged?.Invoke();
        };
    }

    /// <summary>
    /// Gets the main fluid page.
    /// </summary>
    public static FlowNavigation Current
    {
        get => s_current;
        internal set { s_current = value; Loaded?.Invoke(value.View); }
    }

    /// <summary>
    /// Called when the flow navigation is loaded.
    /// </summary>
    public static event Action<IFluidHost>? Loaded;

    /// <summary>
    /// Called when the breakpoint changes.
    /// </summary>
    public static event Action? BreakpointChanged;

    /// <summary>
    /// Called when navigating.
    /// </summary>
    public static event Action<View>? Navigating;

    /// <summary>
    /// Gets the active routes.
    /// </summary>
    public RouteMap ActiveRoutes { get; }

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
    public IFluidHost View { get; }

    /// <summary>
    /// Gets the main page.
    /// </summary>
    public Page Page => (Page)View;

    /// <summary>
    /// Gets the active breakpoint.
    /// </summary>
    public BreakPoint ActiveBreakpoint { get; private set; }

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

    /// <summary>
    /// Shows a modal with a view to get a response of type <typeparamref name="TResponse"/>.
    /// </summary>
    /// <typeparam name="TView"></typeparam>
    /// <typeparam name="TResponse"></typeparam>
    /// <returns></returns>
    /// <exception cref="Exception"></exception>
    public Task<TResponse> Prompt<TView, TResponse>(string? alias = null)
    {
        var targetType = typeof(TView);

        var modalContent = _services.GetService(targetType) as ModalContent<TResponse> ??
            throw new Exception($"View {targetType.Name} not found or is not a modal of type {typeof(TResponse).Name}");

        Current._enteringView = modalContent;

        modalContent.Initialize();
        modalContent.Entering();
        modalContent.OnBreakpointChanged();

        var modal = new ModalView(modalContent) { ZIndex = _zIndex++ };
        AbsoluteLayout.SetLayoutBounds(modal, new(0, 0, Current.View.Width, Current.View.Height));

        _ = new Flow(modal)
          .ToDouble(VisualElement.TranslationYProperty, endValue: 0, fromValue: 600)
          .ToDouble(VisualElement.ScaleXProperty, endValue: 1, fromValue: 0.6)
          .Animate();

        Current.View.AddToRoot(modal);

        modalContent.ResponseTaskCompletionSource = new();

        return modalContent.ResponseTaskCompletionSource.Task;
    }

    /// <summary>
    /// Closes the given modal.
    /// </summary>
    internal async void CloseModal(ModalView modalView)
    {
        var dialog = modalView.Dialog;

        _ = dialog.ScaleXTo(0.7, 500, Easing.CubicOut);
        _ = modalView.FadeTo(0, 500, Easing.CubicOut);
        _ = await dialog.TranslateTo(0, 600, 500, Easing.CubicOut);

        modalView.ModalContent.Leaving();

        Current.View.RemoveFromRoot(modalView);
    }

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

        var host = Current?.View ?? throw new Exception("unable to get current view.");

        nextView.ZIndex = _zIndex++;
        nextView.WidthRequest = host.Width;
        nextView.HeightRequest = host.Height;

        Current.View.AddToPresenter(_currentView = nextView);

        if (nextView is FluidView fluidView)
        {
            _enteringView = fluidView;

            fluidView.Initialize();
            fluidView.Entering();
            fluidView.OnBreakpointChanged();

            if (fluidView.TransitionView is not null)
            {
                var tb = fluidView.TransitionView.TransitionBounds;

                nextView.WidthRequest = tb.Width;
                nextView.HeightRequest = tb.Height;
                nextView.TranslationX = tb.X;
                nextView.TranslationY = tb.Y;

                var flowView = (Page?)Current.View ?? throw new Exception("unable to get current view.");

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

    private static BreakPoint GetBreakpoint()
    {
        var screenWidth = Current.View.Width;

        // sm is the default breakpoint
        var breakPoint = BreakPoint.sm;

        if (screenWidth >= s_screens[1]) breakPoint = BreakPoint.md;
        if (screenWidth >= s_screens[2]) breakPoint = BreakPoint.lg;
        if (screenWidth >= s_screens[3]) breakPoint = BreakPoint.xl;
        if (screenWidth >= s_screens[4]) breakPoint = BreakPoint.xxl;

        return breakPoint;
    }
}
