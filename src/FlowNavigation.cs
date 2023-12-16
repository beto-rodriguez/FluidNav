using System.Diagnostics;

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
    private int _activeIndex;
    private List<string> _navigationStack = [];
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
    public IFluidHost View { get; } = view;

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

        var view = (View?)_services.GetService(targetType) ??
            throw new Exception($"View {targetType.Name} not found");

        if (_activeView is not null) await _activeView.OnLeave();
        Current?.View.ShowView(view);

        if (view is FluidView fluidView)
        {
            if (isHotReload)
            {
                try
                {
                    fluidView.Content = fluidView.GetView();
                }
                catch (Exception ex)
                {
                    Trace.WriteLine($"Failed to hot reload {fluidView.GetType().Name}: {ex.Message}");
                }
            }

            await fluidView.OnEnter();
            _activeView = fluidView;
        }

        return view;
    }
}
