using FluidNav.Flowing;

namespace FluidNav;

public abstract class TransitionView : ContentView
{
    private readonly Dictionary<Type, IEnumerable<Flow>> _flows = [];

    public Rect TransitionBounds { get; set; }

    public void HasTransitionState<TView>(params Flow[] flow)
    {
        _flows[typeof(TView)] = flow;
    }

    public bool RemoveState<TView>()
    {
        return _flows.Remove(typeof(TView));
    }

    public TransitionView FlowToResult<TView>()
    {
        return this.FlowToResult(_flows[typeof(TView)]);
    }

    public TransitionView FlowToResult(Type type)
    {
        return this.FlowToResult(_flows[type]);
    }

    public Task<bool> Flow<TView>()
    {
        return this.Flow(_flows[typeof(TView)]);
    }

    public Task<bool> Flow(Type type)
    {
        return this.Flow(_flows[type]);
    }
}
