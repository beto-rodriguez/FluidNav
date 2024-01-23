namespace FluidNav;

public abstract class ResponsiveView : ContentView, IDisposable
{
    private Rule[] _rules = [];

    public ResponsiveView()
    {
        FlowNavigation.BreakpointChanged += OnBreakpointChanged;
    }

    public virtual void OnBreakpointChanged()
    {
        foreach (var rule in _rules)
        {
            if (!rule.Predicate()) continue;
            rule.Action();
        }
    }

    public void AddRule(BreakPoint breakPoint, Func<bool> predicate, Action action)
    {
        var rules = new Rule[_rules.Length + 1];
        Array.Copy(_rules, rules, _rules.Length);
        rules[^1] = new Rule { BreakPoint = breakPoint, Predicate = predicate, Action = action };
        _rules = [.. rules.OrderBy(x => x.BreakPoint)];
    }

    public void ClearRules() => _rules = [];

    public void Dispose() => FlowNavigation.BreakpointChanged -= OnBreakpointChanged;

    private class Rule
    {
        public required BreakPoint BreakPoint { get; set; }
        public required Func<bool> Predicate { get; set; }
        public required Action Action { get; set; }
    }
}
