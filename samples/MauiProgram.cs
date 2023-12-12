using System.Diagnostics.CodeAnalysis;
using System.Diagnostics;
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
                .AddFont("RobotoMono-VariableFont_wght", "RobotoMono")
                .AddFont("Outfit-VariableFont_wght.ttf", "Outfit")
                .AddFont("icons.ttf", "Icons"))
            .UseFluidNav<FluidHostPage>(r => r
                //.AddRoute<UsersCollection, UsersCollectionVM>()
                .AddRoute<User, UserVM>());

        builder.Services
            .AddSingleton<ICommunityToolkitHotReloadHandler, HotReloadHandler>();

#if DEBUG
        builder.Logging.AddDebug();
#endif

        return builder.Build();
    }
}


