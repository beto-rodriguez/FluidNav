namespace FluidNav;

public abstract class FluidView : ResponsiveView
{
    private Action[] _entering = [];
    private Action[] _leaving = [];

    public FluidView()
    {
        Content = GetView();
        OnBreakpointChanged();
    }

    public TransitionView? TransitionView { get; private set; }

    public abstract View GetView();

    public T UseAsTransition<T>(T view) where T : TransitionView
    {
        TransitionView = view;
        return view;
    }

    public void AddEnteringRule(Action action)
    {
        var rules = new Action[_entering.Length + 1];
        Array.Copy(_entering, rules, _entering.Length);
        rules[^1] = action;
        _entering = rules;
    }

    public void AddLeavingRule(Action action)
    {
        var rules = new Action[_leaving.Length + 1];
        Array.Copy(_leaving, rules, _leaving.Length);
        rules[^1] = action;
        _leaving = rules;
    }

    public void ClearEnteringAndLeavingRules()
    {
        _entering = [];
        _leaving = [];
    }

    internal void Entering()
    {
        OnEntering();
        foreach (var action in _entering) action();
    }

    internal void Leaving()
    {
        OnLeaving();
        foreach (var action in _leaving) action();
    }

    protected virtual void OnEntering() { }

    protected virtual void OnLeaving() { }
}
