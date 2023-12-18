using CommunityToolkit.Maui.Markup;
using FluidNav;
using FluidNav.Flowing;
using Sample.Data;
using static CommunityToolkit.Maui.Markup.GridRowsColumns;

namespace Sample.Views;

public class Playlist(RouteParams routeParams, DataAccessLayer dal) : FluidView<PlaylistTransitionView>
{
    private readonly RouteParams _routeParams = routeParams;
    private readonly DataAccessLayer _dal = dal;
    private ScrollView _scrollView = null!;

    public override View GetView()
    {
        return new ScrollView()
        {
            Content = new VerticalStackLayout()
            {
                new PlaylistTransitionView().Ref(out _transitionView),

                new VerticalStackLayout()
                {
                    MaximumHeightRequest = 600,
                    MinimumWidthRequest = 400,
                    HorizontalOptions = LayoutOptions.Center,
                    Spacing = 15,
                    Children =
                    {
                        GetAlbum("1"),
                        GetAlbum("2"),
                        GetAlbum("3"),
                        GetAlbum("1"),
                        GetAlbum("2"),
                        GetAlbum("3"),
                        GetAlbum("1"),
                        GetAlbum("2"),
                        GetAlbum("3"),
                    }
                }
            }
        }
        .Ref(out _scrollView);
    }

    public override Task OnEnter()
    {
        var idParam = _routeParams["id"];
        var id = int.Parse(idParam);

        BindingContext = _dal.Users.First(u => u.Id == id);

        _ = _transitionView.FlowToResult(v => v.ListViewFlow);
        _ = _transitionView.Flow(v => v.CardViewFlow);

        _transitionView._downloadButton.IsVisible = true;
        _transitionView._moreButton.IsVisible = true;

        _ = _transitionView._root.Flow(v => v.Flows().ToDouble(HeightRequestProperty, 650));
        _ = _transitionView._descriptionLabel.Flow(v => v.Flows().ToDouble(OpacityProperty, 1));

        return Task.CompletedTask;
    }

    public override async Task OnLeave()
    {
        _ = await _transitionView.Flow(v => v.ListViewFlow);

        _ = _scrollView.ScrollToAsync(0, 0, false);
    }

    public static View GetAlbum(string name)
    {
        return new Grid
        {
            RowDefinitions = new([new(Star), new(Star)]),
            ColumnDefinitions = new([new(50), new(Star), new(50)]),
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
}
