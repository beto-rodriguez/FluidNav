using CommunityToolkit.Maui.Core;
using CommunityToolkit.Maui.Core.Platform;
using CommunityToolkit.Maui.Markup;
using FluidNav;
using Sample.Data;
using static CommunityToolkit.Maui.Markup.GridRowsColumns;

namespace Sample.Views;

public class Playlist(RouteParams routeParams, DataAccessLayer dal) : FluidView
{
    public override View GetView() =>
        new ScrollView()
        {
            Content = new VerticalStackLayout()
            {
                new PlaylistTransitionView().AsTransitionFor(this),

                new VerticalStackLayout()
                {
                    MaximumHeightRequest = 600,
                    MinimumWidthRequest = 400,
                    HorizontalOptions = LayoutOptions.Center,
                    Spacing = 15,
                    Children =
                    {
                        GetAlbum("1"), GetAlbum("2"), GetAlbum("3"),
                        GetAlbum("1"), GetAlbum("2"), GetAlbum("3"),
                        GetAlbum("1"), GetAlbum("2"), GetAlbum("3"),
                    }
                }
            }
        }
        .OnEntering(this, scrollView =>
        {
            var id = int.Parse(routeParams["id"]);
            var user = dal.Users.First(u => u.Id == id);

            BindingContext = user;

            _ = scrollView.ScrollToAsync(0, 0, false);

            if (DeviceInfo.Platform == DevicePlatform.WinUI) return;
            StatusBar.SetColor(user.BackgroundColor);
            StatusBar.SetStyle(StatusBarStyle.LightContent);
        });

    public static View GetAlbum(string name) =>
        new Grid
        {
            RowDefinitions = Rows.Define(Star, Star),
            ColumnDefinitions = Columns.Define(50, Star, 50),
            Children =
            {
                new Image()
                    .Row(0)
                    .Column(0)
                    .Source($"album{name}.png")
                    .Aspect(Aspect.AspectFill)
                    .RowSpan(2),

                new Label()
                    .Row(0)
                    .Column(1)
                    .Paddings(left: 10)
                    .Text($"Album name {name}")
                    .FontSize(18)
                    .Bold(),

                new Label()
                    .Row(1)
                    .Column(1)
                    .Paddings(left: 10)
                    .Text($"Artist name {name}")
                    .FontSize(14)
                    .TextColor(Colors.Gray),

                new HorizontalStackLayout
                {
                    Children =
                    {
                        new Button()
                            .Font("Icons")
                            .Text("5")
                            .TextColor(Colors.Gray)
                            .Background(Colors.Transparent),

                        new Button()
                            .Font("Icons")
                            .Text("3")
                            .TextColor(Colors.Gray)
                            .Background(Colors.Transparent)
                    }
                }
                .Row(0)
                .Column(2)
                .RowSpan(2)
            }
        };
}
