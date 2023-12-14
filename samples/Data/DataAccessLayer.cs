using Sample.ViewModels;

namespace Sample.Data;

public class DataAccessLayer
{
    public PlaylistVM[] Users { get; } = [
        new PlaylistVM
        {
            Id = 0,
            Name = "Saxophone House",
            Date = new DateTime(2023, 3, 1),
            Author = "ANNABELLE LUCERO",
            AuthorPicture = "user_avatar.png",
            Description = "Most popular Saxophone House playlist on Spotify since 2013 | Updated weekly | Good vibes only | Photo by Atikh Bana",
            Banner = "sax_player.png",
            BackgroundColor = Color.FromRgba(0, 0, 0, 255),
            TextColor = Color.FromRgba(255, 255, 255, 255)
        },
        new PlaylistVM
        {
            Id = 1,
            Name = "Feel-Good Indie Rock",
            Date = new DateTime(2023, 2, 1),
            Author = "JESSICA HOUSTON",
            AuthorPicture = "user_avatar_2.png",
            Description = "The best indie rock vibes — classic and current. Headphones on | Video by Anna Shvets on pexels.com",
            Banner = "good_vives.png",
            BackgroundColor = Color.FromRgba(247, 223, 241, 255),
            TextColor = Color.FromRgba(139, 104, 156, 255)
        },
        new PlaylistVM
        {
            Id = 2,
            Name = "Peaceful Guitar",
            Date = new DateTime(2022, 12, 1),
            Author = "DAVID HICKMAN",
            AuthorPicture = "user_avatar_3.png",
            Description = "Unwind to these calm classical guitar pieces. Photo by Te NGuyen on Unsplash",
            Banner = "guitar_player.png",
            BackgroundColor = Color.FromRgba(109, 117, 255, 255),
            TextColor = Color.FromRgba(255, 255, 255, 255)
        }
    ];
}
