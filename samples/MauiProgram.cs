using CommunityToolkit.Maui.Markup;
using FluidNav;
using Microsoft.Extensions.Logging;
using Sample.ViewModels;
using Sample.Views;

namespace Sample;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .UseMauiCommunityToolkitMarkup()
            .ConfigureFonts(fonts => fonts
                .AddFont("Outfit-Regular.ttf", "Outfit")
                .AddFont("icons.ttf", "Icons"))
            .UseFluidNav<FluidHostPage>(r => r
                //.AddRoute<AbsoluteTest>()
                .AddRoute<UsersCollection, UsersCollectionVM>()
                .AddRoute<User, UserVM>());

        builder.Services
            .AddSingleton<ICommunityToolkitHotReloadHandler, HotReloadHandler>();

#if DEBUG
        builder.Logging.AddDebug();
#endif

        return builder.Build();
    }
}


