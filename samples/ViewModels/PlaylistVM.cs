using CommunityToolkit.Mvvm.ComponentModel;

namespace Sample.ViewModels;

public class PlaylistVM : ObservableObject
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public DateTime Date { get; set; }
    public required string Author { get; set; }
    public required string AuthorPicture { get; set; }
    public required string Description { get; set; }
    public required string Banner { get; set; }
    public required Color BackgroundColor { get; set; }
    public required Color TextColor { get; set; }
}
