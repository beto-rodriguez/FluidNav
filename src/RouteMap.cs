namespace FluidNav;

/// <summary>
/// Defines a route for the application.
/// </summary>
/// <param name="services">The services collection.</param>
public class RouteMap(IServiceCollection services)
{
    private readonly Dictionary<string, Type> _routes = [];

    /// <summary>
    /// Gets or sets the route type.
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
    public Type this[string key]
    {
        get => _routes[key];
        set => _routes[key] = value;
    }

    /// <summary>
    /// Gets or sets the default route.
    /// </summary>
    public Type? DefaultRoute { get; set; }

    /// <summary>
    /// Creates a route for a view, the view will be registered in the <see cref="IServiceCollection"/> with
    /// a the view as its own BindingContext.
    /// </summary>
    /// <typeparam name="TView">The type of the view.</typeparam>
    /// <param name="route">The route name. If not specified, the view type name in lower case will be used.</param>
    /// <returns>The current map.</returns>
    public RouteMap AddRoute<TView>(string? route = null)
        where TView : ContentView
    {
        return AddRoute<TView, TView>();
    }

    /// <summary>
    /// Creates a route for a view, the view will be registered in the <see cref="IServiceCollection"/> with
    /// a new instance of the view model as the BindingContext.
    /// </summary>
    /// <typeparam name="TView">The type of the view.</typeparam>
    /// <typeparam name="TViewModel">The type of the view model.</typeparam>
    /// <returns>The current map.</returns>
    public RouteMap AddRoute<TView, TViewModel>()
        where TView : ContentView
    {
        var viewType = typeof(TView);
        var viewModelType = typeof(TViewModel);
        var routeName = viewType.Name;

        _routes.Add(routeName, viewType);
        _ = services.AddSingleton(viewModelType);
        _ = services.AddSingleton(viewType);

        DefaultRoute ??= viewType;

        return this;
    }

    /// <summary>
    /// Determines whether the specified route is registered.
    /// </summary>
    /// <param name="route">The route.</param>
    /// <returns>A value indicating whether the route is registered.</returns>
    public bool Contains(string route)
    {
        return _routes.ContainsKey(route);
    }
}
