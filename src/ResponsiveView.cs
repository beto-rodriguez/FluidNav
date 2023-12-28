namespace FluidNav;

public abstract class ResponsiveView : ContentView
{
    private Rule[] _rules = [];

    public ResponsiveView()
    {
        var view = FlowNavigation.Current.View ?? throw new Exception("Host view not found");
        view.BreakpointChanged += OnBreakpointChanged;
    }

    public BreakPoint Breakpoint => FlowNavigation.Current.View.ActiveBreakpoint;

    public virtual void OnBreakpointChanged()
    {
        foreach (var rule in _rules)
        {
            if (!rule.Predicate()) continue;

            rule.Action();
        }
    }

    public void AddRule(Func<bool> predicate, Action action)
    {
        var rules = new Rule[_rules.Length + 1];
        Array.Copy(_rules, rules, _rules.Length);
        rules[^1] = new Rule { Predicate = predicate, Action = action };
        _rules = rules;
    }

    public void ClearRules() => _rules = [];

    private class Rule
    {
        public required Func<bool> Predicate { get; set; }
        public required Action Action { get; set; }
    }
}
