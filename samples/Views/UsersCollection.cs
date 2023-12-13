using CommunityToolkit.Maui.Markup;
using FluidNav;
using FluidNav.Flowing;
using Sample.Data;
using Sample.ViewModels;

namespace Sample.Views;

public class UsersCollection : FluidView
{
    private UserFlowView? _activeUserView;

    public UsersCollection(DataAccessLayer dal)
    {
        var collectionView = new CollectionView
        {
            // Hack to get the CollectionView to scroll to the top always
            Footer = new BoxView().Size(10, 900)
        };

        Content = collectionView
            .ItemsSource(dal.Users)
            .ItemTemplate(new DataTemplate(() =>
            {
                return User
                    .GetFlowView(view =>
                    {
                        _ = view
                            .TapGesture(async () =>
                            {
                                _activeUserView = view;

                                var user = (UserVM)view.BindingContext;
                                collectionView.ScrollTo(user, position: ScrollToPosition.Start);

                                _ = await view.Flow(view.CardViewFlow);

                                var resultView = Fluid.MainView.GoTo<User>();
                                resultView.BindingContext = dal.Users[user.Id];
                            })
                            .FlowToResult(view.ListViewFlow);
                    });
            }));
    }

    public override void OnEnter()
    {
        if (_activeUserView is null) return;

        _ = _activeUserView.Flow(_activeUserView.ListViewFlow);
    }

    public override void OnLeave()
    { }
}
