﻿namespace FluidNav;

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

                var instance = FlowNavigation.Current = new FlowNavigation(provider, host, map);

                if (host is Page hostPage) NavigationPage.SetHasNavigationBar(hostPage, false);
                if (map.DefaultRoute is null) throw new Exception("Default route not set");

                _ = instance.GoTo(map.DefaultRoute);

                return instance;
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
}