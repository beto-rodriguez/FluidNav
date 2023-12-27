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

    public override View GetView()
    {
        var collectionView = new CollectionView
        {
            ItemsLayout = FlowNavigation.Current.View.ActiveBreakpoint >= BreakPoint.lg
                ? _largeScreenLayout
                : _smallScreenLayout
        };

        return collectionView
            .ItemsSource(dal.Users)
            .ItemTemplate(
                TransitionView.Build<PlaylistCollection, Playlist, PlaylistTransitionView>(item =>
                {
                    var user = (PlaylistVM)item;    // <- the item source
                    return $"id={user.Id}";         // <- the route params
                }));
    }

    public override void OnEntering()
    {
#if !WINDOWS
        StatusBar.SetColor(Colors.White);
        StatusBar.SetStyle(StatusBarStyle.DarkContent);
#endif
    }

    public override void OnBreakpointChanged()
    {
        // workaround for  https://github.com/dotnet/maui/issues/7747
        Content = GetView();
    }

    // should work when the next issue is fixed:
    // https://github.com/dotnet/maui/issues/7747

    protected override void OnSm()
    {
        //_collectionView.ItemsLayout = _smallScreenLayout;
    }

    protected override void OnLg()
    {
        //_collectionView.ItemsLayout = _largeScreenLayout;
    }
}
