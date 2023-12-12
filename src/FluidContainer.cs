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
        GoTo(map.DefaultRoute);
    }

    public IFluidPage FluidView { get; }

    /// <summary>
    /// Gets the active routes.
    /// </summary>
    public RouteMap ActiveRoutes { get; }

    /// <summary>
    /// Navigates to the specified route.
    /// </summary>
    /// <param name="route"></param>
    /// <exception cref="Exception"></exception>
    public void GoTo(string route)
    {
        if (!ActiveRoutes.Contains(route)) throw new Exception($"Route {route} not found");

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
        var targetType = ActiveRoutes[_navigationStack[_activeIndex]];
        FluidView.Presenter.Content = (View?)_provider.GetService(targetType);
    }
}
