using CommunityToolkit.Maui.Markup;
using FluidNav;

namespace Sample;

public class FluidHostPage : ContentPage, IFluidHost
{
    private int _zIndex = 0;
    private View? _currentView;
    private readonly AbsoluteLayout _presenter = [];
    private static readonly Dictionary<int, int> s_screens = new()
    {
        { (int)BreakPoint.sm, 640 },
        { (int)BreakPoint.md, 768 },
        { (int)BreakPoint.lg, 1024 },
        { (int)BreakPoint.xl, 1280 },
        { (int)BreakPoint.xxl, 1536 }
    };

    public FluidHostPage()
    {
        Content = _presenter = [];
        Loaded += (_, _) => FlowNavigation.Current.Initialize(this);
        SizeChanged += (_, _) =>
        {
            _ = (_currentView?.Size(Width, Height));

            var bp = GetBreakpoint();
            if (bp == ActiveBreakpoint) return;

            ActiveBreakpoint = bp;
            BreakpointChanged?.Invoke();
        };

        Presenter = _presenter;
    }

    public View Presenter { get; }
    public BreakPoint ActiveBreakpoint { get; private set; } = BreakPoint.sm;

    public event Action? BreakpointChanged;

    public void ShowView(View view)
    {
        view.ZIndex = _zIndex++;
        view.WidthRequest = Width;
        view.HeightRequest = Height;
        _currentView = view;
        if (!_presenter.Children.Contains(view))
            _presenter.Children.Add(view);
    }

    private BreakPoint GetBreakpoint()
    {
        var screenWidth = Width;

        // sm is the default breakpoint
        var breakPoint = BreakPoint.sm;

        if (screenWidth >= s_screens[1]) breakPoint = BreakPoint.md;
        if (screenWidth >= s_screens[2]) breakPoint = BreakPoint.lg;
        if (screenWidth >= s_screens[3]) breakPoint = BreakPoint.xl;
        if (screenWidth >= s_screens[4]) breakPoint = BreakPoint.xxl;

        return breakPoint;
    }
}
