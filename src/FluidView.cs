namespace FluidNav;

public abstract class FluidView : ContentView
{
    public FluidView()
    {
        Content = GetView();
    }

    public abstract View GetView();
    public abstract void OnEnter();
    public abstract void OnLeave();
}
