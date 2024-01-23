namespace FluidNav;

public static partial class ResponsiveExtensions
{
    public static T W25<T>(this T view) where T : View
    {
        FlexLayout.SetBasis(view, new Microsoft.Maui.Layouts.FlexBasis(.25f, true));
        return view;
    }

    public static T W50<T>(this T view) where T : View
    {
        FlexLayout.SetBasis(view, new Microsoft.Maui.Layouts.FlexBasis(.50f, true));
        return view;
    }

    public static T W75<T>(this T view) where T : View
    {
        FlexLayout.SetBasis(view, new Microsoft.Maui.Layouts.FlexBasis(.75f, true));
        return view;
    }

    public static T W100<T>(this T view) where T : View
    {
        FlexLayout.SetBasis(view, new Microsoft.Maui.Layouts.FlexBasis(1f, true));
        return view;
    }

    public static T M<T>(this T view, double size) where T : View
    {
        view.Margin = new Thickness(size);
        return view;
    }

    public static T Ml<T>(this T view, double size) where T : View
    {
        view.Margin = new Thickness(size, view.Margin.Top, view.Margin.Right, view.Margin.Bottom);
        return view;
    }

    public static T Mr<T>(this T view, double size) where T : View
    {
        view.Margin = new Thickness(view.Margin.Left, view.Margin.Top, size, view.Margin.Bottom);
        return view;
    }

    public static T Mt<T>(this T view, double size) where T : View
    {
        view.Margin = new Thickness(view.Margin.Left, size, view.Margin.Right, view.Margin.Bottom);
        return view;
    }

    public static T Mb<T>(this T view, double size) where T : View
    {
        view.Margin = new Thickness(view.Margin.Left, view.Margin.Top, view.Margin.Right, size);
        return view;
    }

    public static T Mx<T>(this T view, double size) where T : View
    {
        view.Margin = new Thickness(size, view.Margin.Top, size, view.Margin.Bottom);
        return view;
    }

    public static T My<T>(this T view, double size) where T : View
    {
        view.Margin = new Thickness(view.Margin.Left, size  , view.Margin.Right, size);
        return view;
    }

    public static T OnSm<T>(this T view, Action<T> action) where T : View =>
        OnScreen(view, BreakPoint.sm, () => action(view));

    public static T OnMd<T>(this T view, Action<T> action) where T : View =>
        OnScreen(view, BreakPoint.md, () => action(view));

    public static T OnLg<T>(this T view, Action<T> action) where T : View =>
        OnScreen(view, BreakPoint.lg, () => action(view));

    public static T OnXl<T>(this T view, Action<T> action) where T : View =>
        OnScreen(view, BreakPoint.xl, () => action(view));

    public static T OnXxl<T>(this T view, Action<T> action) where T : View =>
        OnScreen(view, BreakPoint.xxl, () => action(view));

    public static T OnScreen<T>(
        this T view, BreakPoint breakPoint, Action predicate) where T : View
    {
        var current = FlowNavigation.Current;
        current._enteringView.AddRule(breakPoint, () => current.ActiveBreakpoint >= breakPoint, predicate);
        return view;
    }

    public static T OnBreakPointChanged<T>(this T view, Action<BreakPoint> predicate) where T : View
    {
        FlowNavigation.BreakpointChanged += () =>
        {
            predicate(FlowNavigation.Current.ActiveBreakpoint);
        };

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
