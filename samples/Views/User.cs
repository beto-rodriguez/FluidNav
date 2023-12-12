using CommunityToolkit.Maui.Markup;
using Sample.MarkupHelpers;
using Sample.ViewModels;

namespace Sample.Views;

public class User : ContentView
{
    private readonly Border _avatar = new();
    private readonly Label _nameLabel = new();
    private readonly Label _dateLabel = new();
    private readonly Button _addButton = new();

    private readonly double _cardPadding = 25;
    private readonly double _cardWidth = 400d;
    private readonly double _contentWidth;
    private readonly double _avatarSize = 50;
    private readonly double _avatarStrokeThickness = 2;

    private bool _isExpanded = false;

    public User()
    {
        BindingContext = new UserVM();

        _contentWidth = _cardWidth - _cardPadding * 2;

        Expanded = () => [
            _avatar.StateForLayoutBounds(0, 0),
            _nameLabel.StateForLayoutBounds(_avatarSize + _avatarStrokeThickness * 2 + 20, 0),
            _dateLabel.StateForLayoutBounds(_avatarSize + _avatarStrokeThickness * 2 + 20, 30),
            _addButton.StateForLayoutBounds(_contentWidth - _cardPadding - 10, 0),
            _addButton.StateForDoubleProperty(ScaleProperty, 1)
        ];

        Contracted = () => [
            _avatar.StateForLayoutBounds(_contentWidth * 0.5 - _avatar.Width * 0.5, 0),
            _nameLabel.StateForLayoutBounds(_contentWidth * 0.5 - _nameLabel.Width * 0.5, 60),
            _dateLabel.StateForLayoutBounds(_contentWidth * 0.5 - _dateLabel.Width * 0.5, 80),
            _addButton.StateForLayoutBounds(_contentWidth * 0.5 - _addButton.Width * 0.5 + 25, 25),
            _addButton.StateForDoubleProperty(ScaleProperty, 0.5)
        ];

        Content = new Border()
            .Style(style => style
                .Add(BackgroundColorProperty, Colors.White)
                .Add(WidthRequestProperty, _cardWidth)
                .Add(PaddingProperty, new Thickness(_cardPadding))
                .Add(Border.StrokeThicknessProperty, 0d)
                .Add(Border.StrokeShapeProperty, Shapes.RoundRectangle(40d))
                .Add(MarginProperty, new Thickness(20))
                .Add(ShadowProperty, Shadows.Small()))
            .TapGesture(() =>
            {
                _ = ((_isExpanded = !_isExpanded) ? Expanded : Contracted)()
                    .GetAnimation()
                    .Start(this);
            })
            .Content(GetContent().SetState(Expanded()));
    }

    public View GetContent()
    {
        return
            new StackLayout
            {
                Children =
                {
                    new AbsoluteLayout
                    {
                        Background = Colors.Red,
                        HeightRequest = _avatarSize + _avatarStrokeThickness * 2,
                        WidthRequest = _contentWidth,
                        Children =
                        {
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
                                    .Add(ShadowProperty, Shadows.Small()))
                                .Size(40)
                                .MinSize(10)
                                .Text("+")
                        }
                    }
                }
            };
    }

    public Func<PropertyState[]> Expanded { get; } = () => [];
    public Func<PropertyState[]> Contracted { get; } = () => [];
}
