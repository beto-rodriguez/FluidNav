namespace FluidNav;

public static class ResponsiveExtensions
{
    public static T OnSm<T>(this T view, ResponsiveView responsiveView, Action<T> action) where T : View =>
        OnScreen(view, responsiveView, () => responsiveView.Breakpoint == BreakPoint.sm, () => action(view));

    public static T OnMd<T>(this T view, ResponsiveView responsiveView, Action<T> action) where T : View =>
        OnScreen(view, responsiveView, () => responsiveView.Breakpoint == BreakPoint.md, () => action(view));

    public static T OnLg<T>(this T view, ResponsiveView responsiveView, Action<T> action) where T : View =>
        OnScreen(view, responsiveView, () => responsiveView.Breakpoint == BreakPoint.lg, () => action(view));

    public static T OnXl<T>(this T view, ResponsiveView responsiveView, Action<T> action) where T : View =>
        OnScreen(view, responsiveView, () => responsiveView.Breakpoint == BreakPoint.xl, () => action(view));

    public static T OnXxl<T>(this T view, ResponsiveView responsiveView, Action<T> action) where T : View =>
        OnScreen(view, responsiveView, () => responsiveView.Breakpoint == BreakPoint.xl, () => action(view));

    public static T OnNotSm<T>(this T view, ResponsiveView responsiveView, Action<T> action) where T : View =>
        OnScreen(view, responsiveView, () => responsiveView.Breakpoint != BreakPoint.sm, () => action(view));

    public static T OnNotMd<T>(this T view, ResponsiveView responsiveView, Action<T> action) where T : View =>
        OnScreen(view, responsiveView, () => responsiveView.Breakpoint != BreakPoint.md, () => action(view));

    public static T OnNotLg<T>(this T view, ResponsiveView responsiveView, Action<T> action) where T : View =>
        OnScreen(view, responsiveView, () => responsiveView.Breakpoint != BreakPoint.lg, () => action(view));

    public static T OnNotXl<T>(this T view, ResponsiveView responsiveView, Action<T> action) where T : View =>
        OnScreen(view, responsiveView, () => responsiveView.Breakpoint != BreakPoint.xl, () => action(view));

    public static T OnNotXxl<T>(this T view, ResponsiveView responsiveView, Action<T> action) where T : View =>
        OnScreen(view, responsiveView, () => responsiveView.Breakpoint != BreakPoint.xl, () => action(view));

    public static T OnGreaterThanSm<T>(this T view, ResponsiveView responsiveView, Action<T> action) where T : View =>
        OnScreen(view, responsiveView, () => responsiveView.Breakpoint >= BreakPoint.sm, () => action(view));

    public static T OnGreaterThanMd<T>(this T view, ResponsiveView responsiveView, Action<T> action) where T : View =>
        OnScreen(view, responsiveView, () => responsiveView.Breakpoint >= BreakPoint.md, () => action(view));

    public static T OnGreaterThanLg<T>(this T view, ResponsiveView responsiveView, Action<T> action) where T : View =>
        OnScreen(view, responsiveView, () => responsiveView.Breakpoint >= BreakPoint.lg, () => action(view));

    public static T OnGreaterThanXl<T>(this T view, ResponsiveView responsiveView, Action<T> action) where T : View =>
        OnScreen(view, responsiveView, () => responsiveView.Breakpoint >= BreakPoint.xl, () => action(view));

    public static T OnScreen<T>(
        this T view, ResponsiveView responsiveView, Func<bool> condition, Action predicate) where T : View
    {
        responsiveView.AddRule(condition, predicate);
        return view;
    }

    public static T AsTransitionFor<T>(this T view, FluidView fluidView) where T : TransitionView
    {
        _ = fluidView.UseAsTransition(view);
        return view;
    }

    public static T OnEntering<T>(this T view, FluidView fluidView, Action<T> action) where T : View
    {
        fluidView.AddEnteringRule(() => action(view));
        return view;
    }

    public static T OnLeaving<T>(this T view, FluidView fluidView, Action<T> action) where T : View
    {
        fluidView.AddLeavingRule(() => action(view));
        return view;
    }
}
