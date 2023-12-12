using CommunityToolkit.Mvvm.ComponentModel;
using Sample.Data;

namespace Sample.ViewModels;

public class UserVM : ObservableObject
{
    public string Name { get; set; } = "Carla Morrison";
}
