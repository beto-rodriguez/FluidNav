using FluidNav;
using FluidNav.Flowing;
using CommunityToolkit.Maui.Markup;
using Microsoft.Maui.Layouts;
using Sample.MarkupHelpers;
using Sample.ViewModels;
using Sample.Data;

namespace Sample.Views;

public class User : ContentView
{
    private readonly AbsoluteLayout _titleWrapper = [];
    private readonly Border _avatar = new();
    private readonly Label _nameLabel = new();
    private readonly Label _dateLabel = new();
    private readonly Button _addButton = new();

    private readonly double _avatarSize = 50;
    private readonly double _avatarStrokeThickness = 2;

    public User(RouteParams routeParams)
    {
        var id = int.Parse(routeParams["id"]);
        var user = DataAccessLayer.Users[id];

        BindingContext = user;

        //Content = new Border()
        //{
        //    Style = new Style<Border>()
        //        .Add(BackgroundColorProperty, Colors.White)
        //        .Add(PaddingProperty, new Thickness(25))
        //        .Add(Border.StrokeThicknessProperty, 0d)
        //        .Add(Border.StrokeShapeProperty, Shapes.RoundRectangle(40d))
        //        .Add(MarginProperty, new Thickness(20))
        //        .Add(ShadowProperty, Shadows.Small()),
        Content = new StackLayout
            {
                GetFlowView(v => v.FlowToResult(v.CardViewFlow)),
                new Label()
                    .Text("this is something else...")
            };
        //};
    }

    public static UserFlowView GetFlowView(Action<UserFlowView> builder)
    {
        var instance = new UserFlowView();
        builder(instance);

        return instance;
    }
}

public class UserFlowView : ContentView
{
    public Border Avatar { get; set; } = new();
    public Label NameLabel { get; set; } = new();
    public Label DateLabel { get; set; } = new();
    public Button AddButton { get; set; } = new();

    public UserFlowView()
    {
        var _avatarSize = 50;
        var _avatarStrokeThickness = 2;

        _ = Avatar.LayoutFlags(AbsoluteLayoutFlags.PositionProportional);
        _ = NameLabel.LayoutFlags(AbsoluteLayoutFlags.PositionProportional);
        _ = DateLabel.LayoutFlags(AbsoluteLayoutFlags.PositionProportional);
        _ = AddButton.LayoutFlags(AbsoluteLayoutFlags.PositionProportional);

        ListViewFlow = [
            this.Flows()
                .ToColor(BackgroundColorProperty, Color.FromRgba(255, 255, 255, 0)),
            Avatar.Flows().ToLayoutBounds(0, 0),
            NameLabel.Flows().ToMargin(left: 70, top: 5).ToLayoutBounds(0, 0),
            DateLabel.Flows().ToMargin(left: 70, top: 30).ToLayoutBounds(0, 0),
            AddButton.Flows().ToDouble(ScaleProperty, 1).ToLayoutBounds(1, 0)
        ];

        CardViewFlow = [
            this.Flows()
                .ToColor(BackgroundColorProperty, Color.FromRgba(255, 255, 255, 255)),
            Avatar.Flows().ToLayoutBounds(0.5, 0),
            NameLabel.Flows().ToMargin(top: 50).ToLayoutBounds(0.5, 0.5),
            DateLabel.Flows().ToMargin(top: 80).ToLayoutBounds(0.5, 0.5),
            AddButton.Flows().ToMargin(left: 30).ToDouble(ScaleProperty, 0.5).ToLayoutBounds(0.5, 0.5),
        ];

        Content = new AbsoluteLayout()
        {
             Avatar
                .Style(style => style
                    .Add(Border.StrokeProperty, Colors.LightGray)
                    .Add(Border.StrokeThicknessProperty, _avatarStrokeThickness)
                    .Add(Border.StrokeShapeProperty, Shapes.RoundRectangle(_avatarSize / 2d)))
                .Size(_avatarSize + _avatarStrokeThickness * 2, _avatarSize + _avatarStrokeThickness * 2)
                .Content(
                    new Image()
                        .Size(_avatarSize)
                        .Source("https://live-transitions.pages.dev/user-avatar.jpg")
                        .Aspect(Aspect.AspectFit)),
            NameLabel
                .Style(style => style
                    .Add(Label.TextColorProperty, Color.FromRgba(50, 50, 50, 250))
                    .Add(Label.FontSizeProperty, 18))
                .Bind(
                    Label.TextProperty,
                    getter: (UserVM vm) => vm.Name),
            DateLabel
                .Style(style => style
                    .Add(Label.TextColorProperty, Colors.Black)
                    .Add(Label.FontSizeProperty, 12)
                    .Add(OpacityProperty, 0.75d))
                .LayoutBounds(_avatarSize + _avatarStrokeThickness * 2 + 20, 30)
                .Text("March 2023"),
            AddButton
                .Style(style => style
                    .Add(BackgroundColorProperty, Color.FromRgba(240, 240, 240, 245))
                    .Add(Button.TextColorProperty, Colors.Black)
                    .Add(Button.FontSizeProperty, 18)
                    .Add(Button.FontFamilyProperty, "Icons")
                    .Add(Button.CornerRadiusProperty, 20)
                    .Add(Button.PaddingProperty, new Thickness(0)))
                .Size(40)
                .MinSize(10)
                .Text("+")
        };
    }

    public Flow[] ListViewFlow { get; set; }
    public Flow[] CardViewFlow { get; set; }
}
