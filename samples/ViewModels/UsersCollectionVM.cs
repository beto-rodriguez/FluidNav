using CommunityToolkit.Mvvm.ComponentModel;
using Sample.Data;

namespace Sample.ViewModels;

public partial class UsersCollectionVM : ObservableObject
{
    public UserVM[] Users { get; set; } = DataAccessLayer.Users;
}
