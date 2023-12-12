namespace Sample.MarkupHelpers;

public static class FluidAnimationsExtensions
{
    public static Flow Flows(this VisualElement element)
    {
        return new Flow(element);
    }

    public static Flow ToDouble(this Flow flow, BindableProperty property, double value)
    {
        if (property.ReturnType != typeof(double))
            throw new ArgumentException("Property flow error. Property must be of type double", nameof(property));

        return flow.Add(
            new FlowProperty(
                flow.VisualElement, property, value, () =>
                {
                    var start = (double)flow.VisualElement.GetValue(property);
                    return t => start + t * (value - start);
                }));
    }

    public static Flow ToMargin(this Flow flow, double left = 0, double top = 0, double right = 0, double bottom = 0)
    {
        return flow.ToThickness(View.MarginProperty, left, top, right, bottom);
    }

    public static Flow ToThickness(
        this Flow flow, BindableProperty property, double left = 0, double top = 0, double right = 0, double bottom = 0)
    {
        if (property.ReturnType != typeof(Thickness))
            throw new ArgumentException("Property flow error. Property must be of type Thickness", nameof(property));

        var value = new Thickness(left, top, right, bottom);

        return flow.Add(
            new FlowProperty(
                flow.VisualElement, property, value, () =>
                {
                    var start = (Thickness)flow.VisualElement.GetValue(property);
                    return t => new Thickness(
                        start.Left + t * (value.Left - start.Left),
                        start.Top + t * (value.Top - start.Top),
                        start.Right + t * (value.Right - start.Right),
                        start.Bottom + t * (value.Bottom - start.Bottom));
                }));
    }

    public static Flow ToLayoutBounds(
        this Flow flow, double x, double y)
    {
        return ToLayoutBounds(flow, x, y, AbsoluteLayout.AutoSize, AbsoluteLayout.AutoSize);
    }

    public static Flow ToLayoutBounds(
        this Flow flow, double x, double y, double width, double height)
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
                }));
    }

    /// <summary>
    /// Sets all the flow properties to their target values (without animations).
    /// </summary>
    /// <param name="view">The target view.</param>
    /// <param name="flowCollection"></param>
    /// <returns></returns>
    public static View FlowToResult(this View view, IEnumerable<Flow> flowCollection)
    {
        foreach (var flow in flowCollection)
            foreach (var flowProperty in flow)
                flowProperty.Setter(flowProperty.TargetValue);

        return view;
    }

    /// <summary>
    /// Starts a flow animation and returns a task that completes when the animation ends.
    /// </summary>
    /// <param name="flowCollection">da flow.</param>
    /// <param name="owner">Identifies the owner of the animation. This can be the visual element on which the animation is applied, or another visual element, such as the page</param>
    /// <param name="duration">The duration in milliseconds.</param>
    /// <param name="easing">The easing function.</param>
    /// <param name="fps">Frames per second, default is 60.</param>
    /// <param name="animationName">The animation identifier name, by default a new Guid is used.</param>
    /// <returns>A task that completes when the animations ends.</returns>
    public static Task<bool> Flow(
        this IEnumerable<Flow> flowCollection, VisualElement owner, uint duration = 500,
        Easing? easing = null, uint fps = 60, string? animationName = null)
    {
        var animation = new Animation();

        foreach (var flow in flowCollection)
            foreach (var flowProperty in flow)
                animation.Add(0, 1, flowProperty.GetAnimation());

        animationName ??= $"animation-{Guid.NewGuid()}";
        easing ??= Easing.CubicOut;

        var taskCompletionSource = new TaskCompletionSource<bool>();
        animation.Commit(
            owner, animationName, 1000 / fps, duration, easing, (v, c) => taskCompletionSource.SetResult(c));

        return taskCompletionSource.Task;
    }
}
