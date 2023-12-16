namespace FluidNav;

public abstract class FluidView : ContentView
{
    public FluidView()
    {
        Content = GetView();
    }

    public abstract View GetView();
    public abstract Task OnEnter();
    public abstract Task OnLeave();
}
