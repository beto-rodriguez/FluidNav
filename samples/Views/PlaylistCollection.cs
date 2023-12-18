﻿using CommunityToolkit.Maui.Markup;
using FluidNav;
using FluidNav.Flowing;
using Sample.Data;
using Sample.ViewModels;

namespace Sample.Views;

public class PlaylistCollection(DataAccessLayer dal) : FluidView<PlaylistTransitionView>
{
    private PlaylistTransitionView? _activeUserView;
    private CollectionView _collectionView = new();

    public override View GetView()
    {
        // the footer is a hack to get the CollectionView to scroll to the top always
        return new CollectionView()
        {
            Footer = new BoxView().Size(10, 1000).Background(Colors.Transparent),
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
                    FlowNavigation.Current.GetView<Playlist>().TransitionView.TransitionBounds = new(
                        p.X, p.Y, transitionView.Content.Width, transitionView.Content.Height);
                    transitionView.Opacity = 0;

                    var user = (PlaylistVM)transitionView.BindingContext;
                    _ = FlowNavigation.Current.GoTo<Playlist>($"id={user.Id}");
                })
                .FlowToResult(transitionView.ListViewFlow);

            return transitionView;
        }));
    }

    public override async Task OnEnter()
    {
        if (_activeUserView is null) return;
        _activeUserView.Opacity = 1;
        _ = await _activeUserView.Flow(_activeUserView.ListViewFlow);
    }

    public override Task OnLeave()
    {
        return Task.CompletedTask;
    }
}
