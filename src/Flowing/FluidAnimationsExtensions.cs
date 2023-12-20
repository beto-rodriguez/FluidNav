namespace FluidNav.Flowing;

public static class FluidAnimationsExtensions
{
    public static Flow Flows(this VisualElement element, params (BindableProperty, object value)[] values)
    {
        var flow = new Flow(element);

        foreach (var (property, value) in values)
        {
            if (property.ReturnType == typeof(double))
                _ = flow.ToDouble(property, (double)value);
            else if (property.ReturnType == typeof(Thickness))
                _ = flow.ToThickness(property, (Thickness)value);
            else if (property.ReturnType == typeof(Color))
                _ = flow.ToColor(property, (Color)value);
            else if (property.ReturnType == typeof(Rect))
                _ = flow.ToLayoutBounds(((Rect)value).X, ((Rect)value).Y, ((Rect)value).Width, ((Rect)value).Height);
            else
                throw new ArgumentException(
                    "Property flow error. Property must be of type double, Thickness, Color or Rect", nameof(property));
        }

        return flow;
    }

    public static Flow Flows(this VisualElement element)
    {
        return new Flow(element);
    }

    public static Flow ToDouble(
        this Flow flow, BindableProperty property, double value, double start = 0, double end = 1)
    {
        if (property.ReturnType != typeof(double))
            throw new ArgumentException("Property flow error. Property must be of type double", nameof(property));

        var a = (double)flow.VisualElement.GetValue(property);

        return flow.Add(
            new FlowProperty(
                flow.VisualElement,
                property,
                value,
                () =>
                {
                    var start = (double)flow.VisualElement.GetValue(property);
                    return t => start + t * (value - start);
                },
                start,
                end));
    }

    public static Flow ToMargin(
        this Flow flow, double left = 0, double top = 0, double right = 0, double bottom = 0, double start = 0, double end = 1)
    {
        return flow.ToThickness(View.MarginProperty, new Thickness(left, top, right, bottom), start, end);
    }

    public static Flow ToThickness(
        this Flow flow, BindableProperty property, double left = 0, double top = 0, double right = 0, double bottom = 0, double start = 0, double end = 1)
    {
        return flow.ToThickness(View.MarginProperty, new Thickness(left, top, right, bottom), start, end);
    }

    public static Flow ToThickness(
        this Flow flow, BindableProperty property, Thickness value, double start = 0, double end = 1)
    {
        if (property.ReturnType != typeof(Thickness))
            throw new ArgumentException("Property flow error. Property must be of type Thickness", nameof(property));

        return flow.Add(
            new FlowProperty(
                flow.VisualElement,
                property,
                value,
                () =>
                {
                    var start = (Thickness)flow.VisualElement.GetValue(property);
                    return t => new Thickness(
                        start.Left + t * (value.Left - start.Left),
                        start.Top + t * (value.Top - start.Top),
                        start.Right + t * (value.Right - start.Right),
                        start.Bottom + t * (value.Bottom - start.Bottom));
                },
                start,
                end));
    }

    public static Flow ToColor(
        this Flow flow, BindableProperty property, Color color, double start = 0, double end = 1)
    {
        if (property.ReturnType != typeof(Color))
            throw new ArgumentException("Property flow error. Property must be of type Color", nameof(property));

        return flow.Add(
            new FlowProperty(
            flow.VisualElement,
            property,
            color,
            () =>
            {
                var start = (Color)flow.VisualElement.GetValue(property);
                return t => Color.FromRgba(
                    start.Red + t * (color.Red - start.Red),
                    start.Green + t * (color.Green - start.Green),
                    start.Blue + t * (color.Blue - start.Blue),
                    start.Alpha + t * (color.Alpha - start.Alpha));
            },
            start,
            end));
    }

    public static Flow ToLayoutBounds(
        this Flow flow, double x, double y, double start = 0, double end = 1)
    {
        return flow.ToLayoutBounds(x, y, AbsoluteLayout.AutoSize, AbsoluteLayout.AutoSize, start, end);
    }

    public static Flow ToLayoutBounds(
        this Flow flow, double x, double y, double width, double height, double start = 0, double end = 1)
    {
        return flow.Add(
            new FlowProperty(
                flow.VisualElement,
                AbsoluteLayout.LayoutBoundsProperty,
                new Rect(x, y, width, height),
                () =>
                {
                    var start = (Rect)flow.VisualElement.GetValue(AbsoluteLayout.LayoutBoundsProperty);
                    return t => new Rect(
                        start.X + t * (x - start.X),
                        start.Y + t * (y - start.Y),
                        start.Width + t * (width - start.Width),
                        start.Height + t * (height - start.Height));
                },
                start,
                end));
    }

    /// <summary>
    /// Sets all the flow properties to their target values (without animations).
    /// </summary>
    /// <param name="view">The target view.</param>
    /// <param name="flowBuilder">da flow builder.</param>
    /// <returns></returns>
    public static T FlowToResult<T>(this T view, Func<T, IEnumerable<Flow>> flowBuilder) where T : View
    {
        return view.FlowToResult(flowBuilder(view));
    }

    /// <summary>
    /// Sets all the flow properties to their target values (without animations).
    /// </summary>
    /// <param name="view">The target view.</param>
    /// <param name="flowBuilder">da flow builder.</param>
    /// <returns></returns>
    public static T FlowToResult<T>(this T view, Func<T, Flow> flowBuilder) where T : View
    {
        return view.FlowToResult([flowBuilder(view)]);
    }

    /// <summary>
    /// Sets all the flow properties to their target values (without animations).
    /// </summary>
    /// <param name="view">The target view.</param>
    /// <param name="flowCollection">da flow.</param>
    /// <returns></returns>
    public static T FlowToResult<T>(this T view, IEnumerable<Flow> flowCollection) where T : View
    {
        foreach (var flow in flowCollection)
            foreach (var flowProperty in flow)
                flowProperty.Setter(flowProperty.TargetValue);

        return view;
    }

    /// <summary>
    /// Starts a flow animation and returns a task that completes when the animation ends.
    /// </summary>
    /// <param name="view">The target view.</param>
    /// <param name="flowBuilder">da flow builder.</param>
    /// <param name="owner">Identifies the owner of the animation. This can be the visual element on which the animation is applied, or another visual element, such as the page</param>
    /// <param name="duration">The duration in milliseconds.</param>
    /// <param name="easing">The easing function.</param>
    /// <param name="fps">Frames per second, default is 60.</param>
    /// <param name="animationName">The animation identifier name, by default a new Guid is used.</param>
    /// <returns>A task that completes when the animations ends.</returns>
    public static Task<bool> Flow<T>(
        this T view, Func<T, IEnumerable<Flow>> flowBuilder, VisualElement? owner = null, uint duration = 500,
        Easing? easing = null, uint fps = 60, string? animationName = null)
            where T : View
    {
        return view.Flow(flowBuilder(view), owner, duration, easing, fps, animationName);
    }

    /// <summary>
    /// Starts a flow animation and returns a task that completes when the animation ends.
    /// </summary>
    /// <param name="view">The target view.</param>
    /// <param name="flowBuilder">da flow builder.</param>
    /// <param name="owner">Identifies the owner of the animation. This can be the visual element on which the animation is applied, or another visual element, such as the page</param>
    /// <param name="duration">The duration in milliseconds.</param>
    /// <param name="easing">The easing function.</param>
    /// <param name="fps">Frames per second, default is 60.</param>
    /// <param name="animationName">The animation identifier name, by default a new Guid is used.</param>
    /// <returns>A task that completes when the animations ends.</returns>
    public static Task<bool> Flow<T>(
        this T view, Func<T, Flow> flowBuilder, VisualElement? owner = null, uint duration = 500,
        Easing? easing = null, uint fps = 60, string? animationName = null)
            where T : View
    {
        return view.Flow([flowBuilder(view)], owner, duration, easing, fps, animationName);
    }

    /// <summary>
    /// Starts a flow animation and returns a task that completes when the animation ends.
    /// </summary>
    /// <param name="view">The target view.</param>
    /// <param name="flowCollection">da flow.</param>
    /// <param name="owner">Identifies the owner of the animation. This can be the visual element on which the animation is applied, or another visual element, such as the page</param>
    /// <param name="duration">The duration in milliseconds.</param>
    /// <param name="easing">The easing function.</param>
    /// <param name="fps">Frames per second, default is 60.</param>
    /// <param name="animationName">The animation identifier name, by default a new Guid is used.</param>
    /// <returns>A task that completes when the animations ends.</returns>
    public static Task<bool> Flow(
    this View view, IEnumerable<Flow> flowCollection, VisualElement? owner = null, uint duration = 500,
    Easing? easing = null, uint fps = 60, string? animationName = null)
    {
        var parentAnimation = new Animation();

        foreach (var flow in flowCollection)
            foreach (var flowProperty in flow)
                parentAnimation.Add(
                    flowProperty.Start,
                    flowProperty.End,
                    flowProperty.GetAnimation());

        var taskCompletionSource = new TaskCompletionSource<bool>();

        parentAnimation.Commit(
            owner ?? view,
            animationName ?? $"animation-{Guid.NewGuid()}",
            1000 / fps,
            duration,
            easing ?? Easing.CubicOut,
            (v, c) => taskCompletionSource.SetResult(c));

        return taskCompletionSource.Task;
    }
}
