using CommunityToolkit.Maui.Markup;
using FluidNav;
using Sample.Data;
using Sample.ViewModels;

namespace Sample.Views;

public class PlaylistCollection(DataAccessLayer dal) : FluidView
{
    public override View GetView()
    {
        return new CollectionView()
        {
            ItemsLayout = DeviceInfo.Idiom == DeviceIdiom.Phone
                ? new LinearItemsLayout(ItemsLayoutOrientation.Vertical)
                {
                    ItemSpacing = 15
                }
                : new GridItemsLayout(ItemsLayoutOrientation.Vertical)
                {
                    Span = 2,
                    HorizontalItemSpacing = 10,
                    VerticalItemSpacing = 10
                }
        }
        .ItemsSource(dal.Users)
        .ItemTemplate(
            TransitionView.Navigate<PlaylistCollection, Playlist, PlaylistTransitionView>(item =>
            {
                var user = (PlaylistVM)item;    // <- the item source
                return $"id={user.Id}";         // <- the route params
            }));
    }
}
