namespace FluidNav;

public abstract class FluidView : ResponsiveView
{
    public FluidView()
    {
        Content = GetView();
        OnBreakpointChanged();
    }

    public TransitionView? TransitionView { get; private set; }

    public abstract View GetView();
    public virtual void OnEntering() { }
    public virtual void OnLeaving() { }

    public T UseAsTransition<T>(T view) where T : TransitionView
    {
        TransitionView = view;
        return view;
    }
}
