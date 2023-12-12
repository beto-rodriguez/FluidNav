using CommunityToolkit.Maui.Markup;
using Microsoft.Maui.Layouts;

namespace Sample.Views;

public class AbsoluteTest : ContentView
{
    public AbsoluteTest()
    {
        Content = new AbsoluteLayout
        {
            Background = Colors.White,
            Children =
            {
                new BoxView()
                    .Background(Colors.Blue)
                    .Size(100, 25)
                    .LayoutBounds(0.5, 0)
                    .LayoutFlags(AbsoluteLayoutFlags.PositionProportional),

                new BoxView()
                    .Background(Colors.Green)
                    .Size(25, 100)
                    .LayoutBounds(0, 0.5)
                    .LayoutFlags(AbsoluteLayoutFlags.PositionProportional),

                new BoxView()
                    .Background(Colors.Red)
                    .Size(25, 100)
                    .LayoutBounds(1, 0.5)
                    .LayoutFlags(AbsoluteLayoutFlags.PositionProportional),

                new BoxView()
                    .Background(Colors.Black)
                    .Size(100, 25)
                    .LayoutBounds(0.5, 1)
                    .LayoutFlags(AbsoluteLayoutFlags.PositionProportional),

                new Label()
                    .Text("Hello, World!")
                    .TextColor(Colors.Black)
                    .Font(size: 20)
                    .LayoutBounds(0.5, 0.5)
                    .LayoutFlags(AbsoluteLayoutFlags.PositionProportional)
            }
        };
    }
}
