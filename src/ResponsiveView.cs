namespace FluidNav;

public abstract class ResponsiveView : ContentView
{
    public ResponsiveView()
    {
        var view = FlowNavigation.Current.View ?? throw new Exception("Host view not found");
        view.BreakpointChanged += OnBreakpointChanged;
    }

    public BreakPoint ActiveBreakpoint => FlowNavigation.Current.View.ActiveBreakpoint;

    public virtual void OnBreakpointChanged()
    {
        var bp = (int)FlowNavigation.Current.View.ActiveBreakpoint;

        OnSm();
        if (bp >= 1) OnMd();
        if (bp >= 2) OnLg();
        if (bp >= 3) OnXl();
        if (bp >= 4) OnXxl();
    }

    protected virtual void OnSm() { }

    protected virtual void OnMd() { }

    protected virtual void OnLg() { }

    protected virtual void OnXl() { }

    protected virtual void OnXxl() { }
}
