namespace FluidNav;

public abstract class FluidView : ContentView
{
    protected TransitionView _transitionView = null!;

    public FluidView()
    {
        Content = GetView();
    }

    public TransitionView TransitionView => _transitionView;

    public abstract View GetView();
    public virtual void OnEntering() { }
    public virtual void OnLeaving() { }
}
