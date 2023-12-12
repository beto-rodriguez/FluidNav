using CommunityToolkit.Mvvm.ComponentModel;

namespace Sample.ViewModels;

public class UserVM : ObservableObject
{
    public int Id { get; set; }
    public string Name { get; set; } = "?";
}
