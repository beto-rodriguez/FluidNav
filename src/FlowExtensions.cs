namespace FluidNav;

public static class FlowExtensions
{
    /// <summary>
    /// Adds the fluid navigation service to the service collection.
    /// </summary>
    /// <typeparam name="THostView">The type of the host view.</typeparam>
    /// <param name="builder">The app builder.</param>
    /// <param name="routesBuilder">The routes in the host view.</param>
    /// <returns>The services reference.</returns>
    public static MauiAppBuilder UseFluidNav<THostView>(
        this MauiAppBuilder builder,
        Action<RouteMap> routesBuilder)
        where THostView : IFluidHost
    {
        var map = new RouteMap(builder.Services);
        routesBuilder(map);

        _ = builder.Services
            .AddSingleton(typeof(THostView))
            .AddSingleton(provider =>
            {
                var host = (IFluidHost?)provider.GetService(typeof(THostView))
                    ?? throw new Exception($"Unable to find an {nameof(IFluidHost)} in the app.");

                if (host is Page hostPage) NavigationPage.SetHasNavigationBar(hostPage, false);

                return map.DefaultRoute is null
                    ? throw new Exception("Default route not set")
                    : FlowNavigation.Current = new FlowNavigation(provider, host, map);
            })
            .AddSingleton(typeof(RouteParams));

        return builder;
    }

    /// <summary>
    /// Gets the fluid host view.
    /// </summary>
    /// <param name="services">The app service provider.</param>
    /// <returns>The host view.</returns>
    public static Page GetFluidHost(this IServiceProvider services)
    {
        var host = services.GetService<FlowNavigation>()
            ?? throw new Exception($"Unable to find a registered ${nameof(FlowNavigation)} in the application");

        return (Page)host.View;
    }

    public static T Ref<T>(this T view, out T reference) where T : View
    {
        return reference = view;
    }

    /// <summary>
    /// Sets the content 
    /// </summary>
    /// <param name="border"></param>
    /// <param name="view"></param>
    /// <returns></returns>
    public static Border Content(this Border border, View view)
    {
        border.Content = view;
        return border;
    }

    /// <summary>
    /// Calles an action when the view is tapped, and passes the coordinates of the element in the UI
    /// relative to the Current FlowNavigation host.
    /// </summary>
    /// <param name="view"></param>
    /// <param name="action"></param>
    /// <returns></returns>
    public static T OnTapped<T>(this T view, Action<Point> action) where T : ContentView
    {
        var tapGesture = new TapGestureRecognizer();

        tapGesture.Tapped += (s, e) =>
        {
            var p = e.GetPosition((Page)FlowNavigation.Current.View);
            var p0 = e.GetPosition(view.Content);

            if (p is null || p0 is null) return;

            action(new(p.Value.X - p0.Value.X, p.Value.Y - p0.Value.Y));
        };

        view.GestureRecognizers.Add(tapGesture);

        return view;
    }
}
