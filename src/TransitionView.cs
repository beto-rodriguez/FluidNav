using FluidNav.Flowing;

namespace FluidNav;

public abstract class TransitionView : ResponsiveView
{
    private Type _activeType = typeof(object);
    private readonly Dictionary<Type, IEnumerable<Flow>[]> _flows = [];

    public Rect TransitionBounds { get; set; }

    public void On<TView>(params Flow[] flow)
    {
        SetBreakpointFlow(typeof(TView), BreakPoint.sm, flow);
    }

    public void On<TView>(BreakPoint breakPoint, params Flow[] flow)
    {
        SetBreakpointFlow(typeof(TView), breakPoint, flow);
    }

    public bool RemoveState<TView>()
    {
        return _flows.Remove(typeof(TView));
    }

    public TransitionView Complete(Type type, IEnumerable<Flow>? flow = null)
    {
        _activeType = type;
        return FluidAnimationsExtensions.Complete(this, flow ?? GetBreakpointFlow(type, ActiveBreakpoint));
    }

    public Task<bool> Animate(Type type, IEnumerable<Flow>? flow = null)
    {
        _activeType = type;
        return FluidAnimationsExtensions.Animate(this, flow ?? GetBreakpointFlow(type, ActiveBreakpoint));
    }

    /// <summary>
    /// Gets a template that animates the transition between two views.
    /// </summary>
    /// <typeparam name="TFromView">The type of the view that starts the navigation.</typeparam>
    /// <typeparam name="TToView">The type of the view where the navigation ends.</typeparam>
    /// <typeparam name="TTransitionView">The type of the transition view.</typeparam>
    /// <returns>The data template.</returns>
    public static DataTemplate Build<TFromView, TToView, TTransitionView>(
        Func<object, string>? routeParamsBuilder = null)
            where TTransitionView : TransitionView, new()
            where TFromView : FluidView
            where TToView : FluidView
    {
        return new DataTemplate(() =>
        {
            var transitionView = new TTransitionView();

            _ = transitionView
                .OnTapped(p =>
                {
                    var tv = FlowNavigation.Current.GetView<TToView>().TransitionView;

                    if (tv is not null)
                    {
                        tv.TransitionBounds = new(
                            p.X,
                            p.Y,
                            transitionView.Content.Width,
                            transitionView.Content.Height);
                    }

                    _ = FlowNavigation.Current.GoTo<TToView>(routeParamsBuilder?.Invoke(transitionView.BindingContext));
                })
                .Complete(typeof(TFromView));

            return transitionView;
        });
    }

    protected override void OnBreakpointChanged(BreakPoint breakPoint)
    {
        base.OnBreakpointChanged(breakPoint);

        _ = Complete(_activeType);
    }

    private void SetBreakpointFlow(Type type, BreakPoint breakpoint, IEnumerable<Flow> flow)
    {
        if (!_flows.TryGetValue(type, out var typeFlows))
        {
            typeFlows = new IEnumerable<Flow>[(int)BreakPoint.xxl + 1];
            _flows[type] = typeFlows;
        }

        typeFlows[(int)breakpoint] = flow;
    }

    private IEnumerable<Flow> GetBreakpointFlow(Type type, BreakPoint breakpoint)
    {
        var flowsOnType = _flows[type];
        var i = (int)breakpoint;
        var flow = flowsOnType[i];

        while (flow is null && i > 0)
            flow = flowsOnType[--i];

        return flow ?? throw new Exception($"No flow found for {type.Name} at breakpoint {breakpoint}");
    }
}
