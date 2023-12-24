using FluidNav.Flowing;

namespace FluidNav;

public abstract class TransitionView : ContentView
{
    private BreakPoint _activeBreakpoint = BreakPoint.sm;
    private Type _activeType = typeof(object);
    private readonly Dictionary<Type, IEnumerable<Flow>[]> _flows = [];
    private static readonly Dictionary<int, int> s_screens = new()
    {
        { (int)BreakPoint.sm, 640 },
        { (int)BreakPoint.md, 768 },
        { (int)BreakPoint.lg, 1024 },
        { (int)BreakPoint.xl, 1280 },
        { (int)BreakPoint.xxl, 1536 }
    };

    public TransitionView()
    {
        SizeChanged += (s, e) =>
        {
            var flow = GetBreakpointFlow(_activeType, out var breakPoint);
            if (breakPoint == _activeBreakpoint) return;

            _activeBreakpoint = breakPoint;
            _ = Complete(_activeType);
        };
    }

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
        return FluidAnimationsExtensions.Complete(this, flow ?? GetBreakpointFlow(type, out _activeBreakpoint));
    }

    public Task<bool> Animate(Type type, IEnumerable<Flow>? flow = null)
    {
        _activeType = type;
        return FluidAnimationsExtensions.Animate(this, flow ?? GetBreakpointFlow(type, out _activeBreakpoint));
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

    private void SetBreakpointFlow(Type type, BreakPoint breakpoint, IEnumerable<Flow> flow)
    {
        if (!_flows.TryGetValue(type, out var typeFlows))
        {
            typeFlows = new IEnumerable<Flow>[(int)BreakPoint.xxl + 1];
            _flows[type] = typeFlows;
        }

        typeFlows[(int)breakpoint] = flow;
    }

    private IEnumerable<Flow> GetBreakpointFlow(Type type, out BreakPoint breakPoint)
    {
        var view = FlowNavigation.Current.View ?? throw new Exception("Host view not found");

        var screenWidth = view.Width;
        var ft = _flows[type];

        var flows = ft[(int)BreakPoint.sm];
        breakPoint = BreakPoint.sm;

        bool evaluateBreakpoint(BreakPoint bp)
        {
            if (screenWidth >= s_screens[(int)bp])
            {
                var f = ft![(int)bp];
                flows = f ?? flows;
                return f is not null;
            }

            return false;
        }

        if (evaluateBreakpoint(BreakPoint.md)) breakPoint = BreakPoint.md;
        if (evaluateBreakpoint(BreakPoint.lg)) breakPoint = BreakPoint.lg;
        if (evaluateBreakpoint(BreakPoint.xl)) breakPoint = BreakPoint.xl;
        if (evaluateBreakpoint(BreakPoint.xxl)) breakPoint = BreakPoint.xxl;

        return flows;
    }
}
