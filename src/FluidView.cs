namespace FluidNav;

public abstract class FluidView<TTransitionView> : ContentView, IFluidView
    where TTransitionView : TransitionView
{
    protected TTransitionView _transitionView = null!;

    public FluidView()
    {
        Content = GetView();
    }

    public TTransitionView TransitionView => _transitionView;

    TransitionView IFluidView.TransitionView => _transitionView;

    public abstract View GetView();
    public abstract Task OnEnter();
    public abstract Task OnLeave();
}
