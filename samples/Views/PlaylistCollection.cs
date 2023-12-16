using CommunityToolkit.Maui.Markup;
using FluidNav;
using FluidNav.Flowing;
using Sample.Data;
using Sample.ViewModels;

namespace Sample.Views;

public class PlaylistCollection(DataAccessLayer dal) : FluidView
{
    private PlaylistTransitionView? _activeUserView;
    private CollectionView _collectionView = new();

    public override View GetView()
    {
        var uu = dal.Users;

        // the footer is a hack to get the CollectionView to scroll to the top always
        return new CollectionView()
        {
            Footer = new BoxView().Size(10, 1000).Background(Colors.Transparent),
            ItemsLayout = new LinearItemsLayout(ItemsLayoutOrientation.Vertical) { ItemSpacing = 15 },
        }
            .Ref(out _collectionView)
            .ItemsSource(dal.Users)
            .ItemTemplate(new DataTemplate(() =>
            {
                var transitionView = new PlaylistTransitionView();

                transitionView._downloadButton.IsVisible = false;
                transitionView._moreButton.IsVisible = false;

                _ = transitionView
                    .OnTapped(p =>
                    {
                        _activeUserView = transitionView;
                        var user = (PlaylistVM)transitionView.BindingContext;
                        var v = FlowNavigation.Current.GetView<Playlist>();
                        v._transitionView.StartPoint = p;
                        _ = FlowNavigation.Current.GoTo<Playlist>($"id={user.Id}");
                    })
                    .FlowToResult(transitionView.ListViewFlow);

                return transitionView;
            }));
    }

    public override Task OnEnter()
    {
        if (_activeUserView is null) return Task.CompletedTask;
        _ = _activeUserView.Flow(_activeUserView.ListViewFlow);

        return Task.CompletedTask;
    }

    public override Task OnLeave()
    {
        return Task.CompletedTask;
    }
}
