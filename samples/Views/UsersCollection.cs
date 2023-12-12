using CommunityToolkit.Maui.Markup;
using Sample.MarkupHelpers;
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
                var user = new User();
                return user.GetContent().FlowToResult(user.Expanded);
            }));
    }
}
