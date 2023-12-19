using CommunityToolkit.Maui.Markup;
using FluidNav;

namespace Sample;

public class FluidHostPage : ContentPage, IFluidHost
{
    private int _zIndex = 0;
    private View? _currentView;
    private readonly AbsoluteLayout _root = [];

    public FluidHostPage()
    {
        Content = _root = [];

        Loaded += (_, _) => FlowNavigation.Current.Initialize(this);
        SizeChanged += (_, _) => _currentView?.Size(Width, Height);

    }

    public void ShowView(View view)
    {
        view.ZIndex = _zIndex++;
        view.WidthRequest = Width;
        view.HeightRequest = Height;

        _currentView = view;

        if (!_root.Children.Contains(view))
            _root.Children.Add(view);
    }

    public enum Row { NavBar, Content }
}
