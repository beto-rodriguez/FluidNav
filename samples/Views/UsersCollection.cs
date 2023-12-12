using CommunityToolkit.Maui.Markup;
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
                return new User().GetContent();
            }));
    }
}
