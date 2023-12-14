using CommunityToolkit.Maui.Markup;
using FluidNav;
using static CommunityToolkit.Maui.Markup.GridRowsColumns;

namespace Sample;

public class FluidHostPage : ContentPage, IFluidHost
{
    private View? _currentView;
    private readonly Grid _root = [];

    public FluidHostPage()
    {
        _root = new Grid
        {
            RowDefinitions = Rows.Define(
                (Row.NavBar, 50),
                (Row.Content, Star)),

            Children =
            {
                new HorizontalStackLayout
                {
                    BackgroundColor = Colors.White,
                    Shadow = new Shadow { Brush = new SolidColorBrush(Colors.Black), Opacity = 0.2f, Radius = 5 },
                    Children =
                    {
                        new Button()
                            .Text("Back")
                            .TapGesture(() => _ = FlowNavigation.Current.GoBack())
                    }
                }
                .Row(Row.NavBar)
            }
        };

        Content = _root;
    }

    public void ShowView(View view)
    {
        _ = _root.Children.Remove(_currentView);
        _currentView = view;

        Grid.SetRow(view, 1);
        _root.Children.Add(_currentView);
    }

    public enum Row { NavBar, Content }
}
