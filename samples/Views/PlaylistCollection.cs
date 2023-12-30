using CommunityToolkit.Maui.Core;
using CommunityToolkit.Maui.Core.Platform;
using CommunityToolkit.Maui.Markup;
using FluidNav;
using Sample.Data;
using Sample.ViewModels;

namespace Sample.Views;

public class PlaylistCollection(DataAccessLayer dal) : FluidView
{
    private readonly GridItemsLayout _largeScreenLayout = new(ItemsLayoutOrientation.Vertical)
    {
        Span = 2,
        HorizontalItemSpacing = 10,
        VerticalItemSpacing = 10
    };

    private readonly LinearItemsLayout _smallScreenLayout = new(ItemsLayoutOrientation.Vertical)
    {
        ItemSpacing = 15
    };

    public override View GetView() =>
        new CollectionView
        {
            ItemsLayout = Breakpoint >= BreakPoint.lg ? _largeScreenLayout : _smallScreenLayout
        }
        .ItemsSource(dal.Users)
        .ItemTemplate(
            // not working properly on mac
            // https://github.com/dotnet/maui/issues/19329
            TransitionView.Build<PlaylistCollection, Playlist, PlaylistTransitionView>(item =>
            {
                var user = (PlaylistVM)item;    // <- the item source
                return $"id={user.Id}";         // <- the route params
            }))
        .OnEntering(this, v =>
        {
            if (DeviceInfo.Idiom == DeviceIdiom.Desktop) return;
            StatusBar.SetColor(Colors.White);
            StatusBar.SetStyle(StatusBarStyle.DarkContent);
        });

    public override void OnBreakpointChanged() => Content = GetView();
}
