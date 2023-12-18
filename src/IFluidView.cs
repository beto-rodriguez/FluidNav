namespace FluidNav;

public interface IFluidView
{
    TransitionView? TransitionView { get; }
    View Content { get; set; }

    public View GetView();
    public Task OnEnter();
    public Task OnLeave();
}
