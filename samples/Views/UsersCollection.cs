using CommunityToolkit.Maui.Markup;
using FluidNav;
using FluidNav.Flowing;
using Sample.ViewModels;

namespace Sample.Views;

public class UsersCollection : ContentView
{
    public UsersCollection(UsersCollectionVM viewModel)
    {
        Content = new CollectionView()
            .ItemsSource(viewModel.Users)
            .ItemTemplate(new DataTemplate(() =>
            {
                return User
                    .GetFlowView(view =>
                    {
                        _ = view
                            .TapGesture(async () =>
                            {
                                var user = (UserVM)view.BindingContext;
                                view.ZIndex = int.MaxValue;
                                _ = await view.Flow(view.CardViewFlow);
                                //Fluid.MainView.GoTo<User>($"id={user.Id}");
                            })
                            .FlowToResult(view.ListViewFlow);
                    });
            }));
    }
}
