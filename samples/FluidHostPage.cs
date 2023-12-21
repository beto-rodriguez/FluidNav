using CommunityToolkit.Maui.Markup;
using FluidNav;

namespace Sample;

public class FluidHostPage : ContentPage, IFluidHost
{
    private int _zIndex = 0;
    private View? _currentView;
    private readonly AbsoluteLayout _presenter = [];

    public FluidHostPage()
    {
        Content = _presenter = [];
        Loaded += (_, _) => FlowNavigation.Current.Initialize(this);
        SizeChanged += (_, _) => _currentView?.Size(Width, Height);

        Presenter = _presenter;
    }

    public View Presenter { get; }

    public void ShowView(View view)
    {
        view.ZIndex = _zIndex++;
        view.WidthRequest = Width;
        view.HeightRequest = Height;
        _currentView = view;
        if (!_presenter.Children.Contains(view))
            _presenter.Children.Add(view);
    }
}
