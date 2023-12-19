namespace FluidNav;

public abstract class FluidView : ContentView
{
    public FluidView()
    {
        Content = GetView();
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
