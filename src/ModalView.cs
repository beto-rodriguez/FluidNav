using Microsoft.Maui.Controls.Shapes;

namespace FluidNav;

public class ModalView : ResponsiveView
{
    public ModalView(ModalContent content, double width = 600)
    {
        var view = FlowNavigation.Current.View;

        ((ContentPage)view).SizeChanged += (_, _) =>
            AbsoluteLayout.SetLayoutBounds(this, new(0, 0, view.Width, view.Height));

        content.ModalView = this;

        //HorizontalOptions = LayoutOptions.Start;
        VerticalOptions = LayoutOptions.Start;
        BackgroundColor = Color.FromRgba(255, 255, 255, 0.50);
        Margin = new Thickness(0);
        Content = new Border
        {
            StrokeThickness = 0,
            Content = new ScrollView
            {
                Content = Dialog = new Border
                {
                    MaximumWidthRequest = width,
                    StrokeThickness = 0,
                    BackgroundColor = Colors.White,
                    StrokeShape = new RoundRectangle { CornerRadius = 6 },
                    Shadow = new Shadow
                    {
                        Brush = new SolidColorBrush(Colors.Black),
                        Opacity = 0.15f,
                        Offset = new Point(0, 0),
                        Radius = 10
                    },
                    Margin = new Thickness(0, 65, 0, 100),
                    Content = new StackLayout
                    {
                        Children =
                        {
                            new Grid
                            {
                                Children =
                                {
                                    new Button
                                    {
                                        ZIndex = 1,
                                        Text = "x",
                                        FontFamily = "flownav_icons",
                                        VerticalOptions = LayoutOptions.Start,
                                        HorizontalOptions = LayoutOptions.End,
                                        Shadow = new Shadow { Brush = null }, //<- override theme shadow
                                        BorderColor = null,
                                        Background = null,
                                        BackgroundColor = Colors.White,
                                        Opacity = 0.5f,
                                        Margin = new Thickness(5),
                                        Command = new Command(content.CloseModal)
                                    },
                                    new Border
                                    {
                                        StrokeThickness = 0,
                                        Padding = new Thickness(16, 16, 16, 16),
                                        Content = ModalContent = content
                                    }
                                }
                            }
                        }
                    }
                }
            }
        };
    }

    public ModalContent ModalContent { get; }
    public View Dialog { get; }
}
