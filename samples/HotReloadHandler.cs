using System.Diagnostics;
using CommunityToolkit.Maui.Markup;
using FluidNav;

namespace Sample;

// https://learn.microsoft.com/en-us/dotnet/communitytoolkit/maui/markup/dotnet-hot-reload#advanced-usage

public class HotReloadHandler : ICommunityToolkitHotReloadHandler
{
    public void OnHotReload(IReadOnlyList<Type> types)
    {
        if (Application.Current?.Windows is null)
        {
            Trace.WriteLine(
                $"{nameof(HotReloadHandler)} Failed: " +
                $"{nameof(Application)}.{nameof(Application.Current)}.{nameof(Application.Current.Windows)} is null");

            return;
        }

        foreach (var window in Application.Current.Windows)
        {
            if (window.Page is not Page currentPage) return;

            _ = currentPage.Dispatcher.Dispatch(FlowNavigation.Current.OnHotReloaded);
        }
    }
}
