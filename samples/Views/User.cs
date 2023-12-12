using CommunityToolkit.Maui.Markup;
using Microsoft.Maui.Layouts;
using Sample.MarkupHelpers;
using Sample.ViewModels;

namespace Sample.Views;

public class User : ContentView
{
    private readonly View _content;

    private readonly AbsoluteLayout _titleWrapper = [];
    private readonly Border _avatar = new();
    private readonly Label _nameLabel = new();
    private readonly Label _dateLabel = new();
    private readonly Button _addButton = new();

    //private readonly double _cardPadding = 25;
    //private readonly double _cardWidth = 400d;
    //private readonly double _contentWidth;
    private readonly double _avatarSize = 50;
    private readonly double _avatarStrokeThickness = 2;

    private bool _isExpanded = false;

    public User()
    {
        BindingContext = new UserVM();

        _ = _avatar.LayoutFlags(AbsoluteLayoutFlags.PositionProportional);
        _ = _nameLabel.LayoutFlags(AbsoluteLayoutFlags.PositionProportional);
        _ = _dateLabel.LayoutFlags(AbsoluteLayoutFlags.PositionProportional);
        _ = _addButton.LayoutFlags(AbsoluteLayoutFlags.PositionProportional);

        Expanded = [
            _avatar.Flows().ToLayoutBounds(0, 0),
            _nameLabel.Flows().ToMargin(left: 70, top: 5).ToLayoutBounds(0, 0),
            _dateLabel.Flows().ToMargin(left: 70, top: 30).ToLayoutBounds(0, 0),
            _addButton.Flows().ToDouble(ScaleProperty, 1).ToLayoutBounds(1, 0)
        ];

        Contracted = [
            _avatar.Flows().ToLayoutBounds(0.5, 0),
            _nameLabel.Flows().ToMargin(top: 50).ToLayoutBounds(0.5, 0.5),
            _dateLabel.Flows().ToMargin(top: 80).ToLayoutBounds(0.5, 0.5),
            _addButton.Flows().ToMargin(left: 30).ToDouble(ScaleProperty, 0.5).ToLayoutBounds(0.5, 0.5),
        ];

        Content = new Border()
            .Style(style => style
                .Add(BackgroundColorProperty, Colors.White)
                //.Add(WidthRequestProperty, _cardWidth)
                .Add(PaddingProperty, new Thickness(25))
                .Add(Border.StrokeThicknessProperty, 0d)
                .Add(Border.StrokeShapeProperty, Shapes.RoundRectangle(40d))
                .Add(MarginProperty, new Thickness(20))
                .Add(ShadowProperty, Shadows.Small()))
            //.TapGesture(() =>
            //{
            //    _ = ((_isExpanded = !_isExpanded) ? Expanded : Contracted)
            //        .GetAnimation()
            //        .Start(this);
            //})
            .Content(_content = GetContent());

        Loaded += (s, e) =>
        {
            _ = _content.FlowToResult(Expanded);
        };
    }

    public View GetContent()
    {
        return
            new StackLayout
            {
                Children =
                {
                    new AbsoluteLayout()
                        .Background(Colors.Red)
                        .TapGesture(() =>
                        {
                            _ = ((_isExpanded = !_isExpanded) ? Contracted : Expanded)
                                   .Flow(this);
                        })
                        .Children(
                            _avatar
                                .Style(style => style
                                    .Add(Border.StrokeProperty, Colors.LightGray)
                                    .Add(Border.StrokeThicknessProperty, _avatarStrokeThickness)
                                    .Add(Border.StrokeShapeProperty, Shapes.RoundRectangle(_avatarSize / 2d)))
                                .Size(_avatarSize + _avatarStrokeThickness * 2, _avatarSize + _avatarStrokeThickness * 2)
                                .Content(
                                    new Image()
                                        .Size(_avatarSize, _avatarSize)
                                        .Source("https://live-transitions.pages.dev/user-avatar.jpg")
                                        .Aspect(Aspect.AspectFit)
                                        .Size(_avatarSize, _avatarSize)),

                            _nameLabel
                                .Style(style => style
                                    .Add(Label.TextColorProperty, Color.FromRgba(50, 50, 50, 250))
                                    .Add(Label.FontSizeProperty, 18))
                                .Bind(
                                    Label.TextProperty,
                                    getter: (UserVM vm) => vm.Name),

                            _dateLabel
                                .Style(style => style
                                    .Add(Label.TextColorProperty, Colors.Black)
                                    .Add(Label.FontSizeProperty, 12)
                                    .Add(OpacityProperty, 0.75d))
                                .LayoutBounds(_avatarSize + _avatarStrokeThickness * 2 + 20, 30)
                                .Text("March 2023"),

                            _addButton
                                .Style(style => style
                                    .Add(BackgroundColorProperty, Color.FromRgba(240, 240, 240, 245))
                                    .Add(Button.TextColorProperty, Colors.Black)
                                    .Add(Button.FontSizeProperty, 18)
                                    .Add(Button.FontFamilyProperty, "Icons")
                                    .Add(Button.CornerRadiusProperty, 20)
                                    .Add(Button.PaddingProperty, new Thickness(0))
                                    //.Add(ShadowProperty, Shadows.Small())
                                    )
                                .Size(40)
                                .MinSize(10)
                                .Text("+"))
                }
            };
    }

    public Flow[] Expanded { get; set; }
    public Flow[] Contracted { get; set; }
}
