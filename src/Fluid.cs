namespace FluidNav;

/// <summary>
/// Defines the fluid navigation helper class.
/// </summary>
public static class Fluid
{
    /// <summary>
    /// Gets the main fluid view.
    /// </summary>
    public static FluidContainer MainView { get; internal set; } = null!;

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
        where THostView : IFluidPage
    {
        var map = new RouteMap(builder.Services);
        routesBuilder(map);

        _ = builder.Services
            .AddSingleton(typeof(THostView))
            .AddSingleton(provider =>
            {
                var host = (IFluidPage?)provider.GetService(typeof(THostView))
                    ?? throw new Exception($"Unable to find an {nameof(IFluidPage)} in the app.");

                return MainView = new FluidContainer(provider, host, map);
            });

        return builder;
    }

    /// <summary>
    /// Gets the fluid host view.
    /// </summary>
    /// <param name="services">The app service provider.</param>
    /// <returns>The host view.</returns>
    public static Page GetFluidHost(this IServiceProvider services)
    {
        var container = services.GetService<FluidContainer>()
            ?? throw new Exception($"Unable to find a registered ${nameof(FluidContainer)} in the application");

        return (Page)container.FluidView;
    }
}
