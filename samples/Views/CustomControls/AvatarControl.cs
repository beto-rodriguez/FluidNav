using CommunityToolkit.Maui.Markup;
using Microsoft.Maui.Controls.Shapes;

namespace Sample.Views.CustomControls;

public class AvatarControl : Border
{
    private readonly Image _image = new();

    public AvatarControl()
    {
        Stroke = new SolidColorBrush(Colors.Gray);
        StrokeThickness = 2;
        StrokeShape = new RoundRectangle { CornerRadius = new(50) };

        WidthRequest = 50 + 2 + 2; // 50 size + 2 stroke left + 2 stroke right
        HeightRequest = 54;

        Content = _image
            .Size(50)
            .Aspect(Aspect.AspectFit);
    }

    public static readonly BindableProperty AvatarImageProperty =
        BindableProperty.Create(nameof(AvatarImage), typeof(ImageSource), typeof(AvatarControl), null,
            propertyChanged: OnAvatarImageChanged);

    public ImageSource AvatarImage
    {
        get => (string)GetValue(AvatarImageProperty);
        set => SetValue(AvatarImageProperty, value);
    }

    private static void OnAvatarImageChanged(BindableObject bindable, object oldValue, object newValue)
    {
        var avatar = (AvatarControl)bindable;
        avatar._image.Source = (ImageSource?)newValue;
    }
}
