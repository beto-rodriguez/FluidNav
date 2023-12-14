using FluidNav;

namespace Sample;

public class FluidHostPage : ContentPage, IFluidHost
{
    private View? _currentView;
    private readonly Grid _root = [];

    public FluidHostPage()
    {
        Content = _root = [];
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
