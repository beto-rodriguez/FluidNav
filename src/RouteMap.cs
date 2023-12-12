namespace FluidNav;

/// <summary>
/// Defines a route for the application.
/// </summary>
/// <param name="services"></param>
public class RouteMap(IServiceCollection services)
{
    private readonly Dictionary<string, Type> _routes = [];

    public Type this[string key]
    {
        get => _routes[key];
        set => _routes[key] = value;
    }

    /// <summary>
    /// Gets or sets the default route.
    /// </summary>
    public string DefaultRoute { get; set; } = string.Empty;

    /// <summary>
    /// Creates a route for a view, the view will be registered in the <see cref="IServiceCollection"/> with
    /// a the view as its own BindingContext.
    /// </summary>
    /// <typeparam name="TView">The type of the view.</typeparam>
    /// <param name="route">The route name. If not specified, the view type name in lower case will be used.</param>
    /// <returns>The current map.</returns>
    public RouteMap AddRoute<TView>(string? route = null)
        where TView : ContentView, new()
    {
        return AddRoute<TView, TView>();
    }

    /// <summary>
    /// Creates a route for a view, the view will be registered in the <see cref="IServiceCollection"/> with
    /// a new instance of the view model as the BindingContext.
    /// </summary>
    /// <typeparam name="TView">The type of the view.</typeparam>
    /// <typeparam name="TViewModel">The type of the view model.</typeparam>
    /// <param name="route">The route name. If not specified, the view type name in lower case will be used.</param>
    /// <returns>The current map.</returns>
    public RouteMap AddRoute<TView, TViewModel>(string? route = null)
        where TView : ContentView
    {
        var viewType = typeof(TView);
        var viewModelType = typeof(TViewModel);
        var routeName = route ?? viewType.Name.ToLower();

        //var isOwnContext = viewType == viewModelType;
        //if (!isOwnContext) _ = services.AddTransient(viewModelType);
        //_ = services
        //    .AddTransient(provider =>
        //    {
        //        var view = new TView();

        //        view.BindingContext = isOwnContext
        //            ? view
        //            : (TViewModel?)provider.GetService(viewModelType);

        //        return view;
        //    });

        _routes.Add(routeName, viewType);
        _ = services.AddTransient(viewModelType);
        _ = services.AddTransient(viewType);

        if (string.IsNullOrWhiteSpace(DefaultRoute)) DefaultRoute = routeName;

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
