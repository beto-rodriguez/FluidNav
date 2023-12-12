namespace FluidNav;

/// <summary>
/// Defines a fluid container.
/// </summary>
public class FluidContainer
{
    private readonly IServiceProvider _provider;
    private int _activeIndex;
    private List<string> _navigationStack = [];

    /// <summary>
    /// Initializes a new instance of the <see cref="FluidContainer"/> class.
    /// </summary>
    /// <param name="provider">The service provider.</param>
    /// <param name="view">The view.</param>
    /// <param name="map">The map.</param>
    public FluidContainer(IServiceProvider provider, IFluidPage view, RouteMap map)
    {
        _provider = provider;
        FluidView = view;
        ActiveRoutes = map;

        NavigationPage.SetHasNavigationBar((Page)view, false);

        if (map.DefaultRoute is null) throw new Exception("Default route not set");
        GoTo(map.DefaultRoute);
    }

    public IFluidPage FluidView { get; }

    /// <summary>
    /// Gets the active routes.
    /// </summary>
    public RouteMap ActiveRoutes { get; }

    /// <summary>
    /// Navigates to the specified type.
    /// </summary>
    /// <typeparam name="TRoute">The type of the route.</typeparam>
    /// <param name="queryParams">The query parameters, e.g. id=10&name=john.</param>
    public void GoTo<TRoute>(string? queryParams = null)
    {
        GoTo(typeof(TRoute).Name + (queryParams is null ? string.Empty : $"?{queryParams}"));
    }

    /// <summary>
    /// Navigates to the specified view type.
    /// </summary>
    /// <param name="type">The type of the route.</param>
    /// <param name="queryParams">The query parameters, e.g. id=10&name=john.</param>
    public void GoTo(Type type, string? queryParams = null)
    {
        GoTo(type.Name + (queryParams is null ? string.Empty : $"?{queryParams}"));
    }

    public void GoTo(string route)
    {
        var routeName = route.Split('?')[0];

        if (!ActiveRoutes.Contains(routeName)) throw new Exception($"Route {routeName} not found");

        _navigationStack = _navigationStack.Take(_activeIndex + 1).ToList();
        _navigationStack.Add(route);
        _activeIndex = _navigationStack.Count - 1;

        Go();
    }

    /// <summary>
    /// Navigates back.
    /// </summary>
    public void GoBack()
    {
        _activeIndex--;
        if (_activeIndex < 0) _activeIndex = 0;

        Go();
    }

    /// <summary>
    /// Navigates forward.
    /// </summary>
    public void GoNext()
    {
        _activeIndex++;
        if (_activeIndex >= _navigationStack.Count) _activeIndex = _navigationStack.Count - 1;

        Go();
    }

    /// <summary>
    /// Refreshes the current view.
    /// </summary>
    public void Refresh()
    {
        FluidView.Presenter.Content = null;
        Go();
    }

    private void Go()
    {
        var routeParts = _navigationStack[_activeIndex].Split('?');

        var routeName = routeParts[0];
        var queryParams = routeParts.Length > 1 ? routeParts[1] : string.Empty;

        var routeParams = (RouteParams?)_provider.GetService(typeof(RouteParams));
        routeParams?.Clear();
        if (queryParams.Length > 0) routeParams?.SetParams(queryParams);

        var targetType = ActiveRoutes[routeName];
        FluidView.Presenter.Content = (View?)_provider.GetService(targetType);
    }
}
