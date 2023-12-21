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
        return this.Complete(_flows[typeof(TView)]);
    }

    public TransitionView FlowToResult(Type type)
    {
        return this.Complete(_flows[type]);
    }

    public Task<bool> Flow<TView>()
    {
        return this.Animate(_flows[typeof(TView)]);
    }

    public Task<bool> Flow(Type type)
    {
        return this.Animate(_flows[type]);
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
                .FlowToResult<TFromView>();

            return transitionView;
        });
    }
}
