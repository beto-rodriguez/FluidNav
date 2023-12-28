namespace FluidNav.Flowing;

public static class FluidAnimationsExtensions
{
    /// <summary>
    /// Defines the flow properties for the given view.
    /// </summary>
    /// <param name="view">the view.</param>
    /// <param name="values">The property/value pair.</param>
    /// <returns>da flow.</returns>
    public static Flow Flows(this View view, params (BindableProperty, object value)[] values)
    {
        var flow = new Flow(view);

        foreach (var (property, value) in values)
        {
            if (property.ReturnType == typeof(double))
            {
                _ = flow.ToDouble(property, Convert.ToDouble(value));
            }
            else if (property.ReturnType == typeof(Thickness))
            {
                _ = value.IsNumber()
                    ? flow.ToThickness(property, new Thickness(Convert.ToDouble(value)))
                    : flow.ToThickness(property, (Thickness)value);
            }
            else if (property.ReturnType == typeof(Color))
            {
                _ = flow.ToColor(property, (Color)value);
            }
            else if (property == AbsoluteLayout.LayoutBoundsProperty)
            {
#pragma warning disable IDE0045 // Convert to conditional expression
                if (value is Rect rect)
                {
                    _ = flow.ToLayoutBounds(rect);
                }
                else if (value is Point point)
                {
                    _ = flow.ToLayoutBounds(new Rect(point.X, point.Y, AbsoluteLayout.AutoSize, AbsoluteLayout.AutoSize));
                }
                else
                {
                    throw new ArgumentException(
                        $"The given type is not supported for the LayoutBoundsProperty property");
                }
#pragma warning restore IDE0045 // Convert to conditional expression
            }
            else
            {
                throw new ArgumentException($"The given type is not supported.", nameof(property));
            }
        }

        return flow;
    }

    /// <summary>
    /// Sets all the flow properties to their target values (without animations).
    /// </summary>
    /// <param name="view">The target view.</param>
    /// <param name="flowCollection">da flow.</param>
    /// <returns></returns>
    public static T Complete<T>(this Flow flow) where T : View => (T)flow.View.Complete([flow]);

    /// <summary>
    /// Sets all the flow properties to their target values (without animations).
    /// </summary>
    /// <param name="view">The target view.</param>
    /// <param name="flowCollection">da flow.</param>
    /// <returns></returns>
    public static T Complete<T>(this T view, IEnumerable<Flow> flowCollection) where T : View
    {
        foreach (var flow in flowCollection)
            foreach (var flowProperty in flow)
                flowProperty.Setter(flowProperty.TargetValue);

        return view;
    }

    /// <summary>
    /// Starts a flow animation and returns a task that completes when the animation ends.
    /// </summary>
    /// <param name="flow">da flow.</param>
    /// <param name="owner">Identifies the owner of the animation. This can be the visual element on which the animation is applied, or another visual element, such as the page</param>
    /// <param name="duration">The duration in milliseconds.</param>
    /// <param name="easing">The easing function.</param>
    /// <param name="fps">Frames per second, default is 60.</param>
    /// <param name="animationName">The animation identifier name, by default a new Guid is used.</param>
    /// <returns>A task that completes when the animations ends.</returns>
    public static Task<bool> Animate(
        this Flow flow, VisualElement? owner = null, uint duration = 500,
        Easing? easing = null, uint fps = 60, string? animationName = null) => flow.View.Animate([flow], owner, duration, easing, fps, animationName);

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
    public static Task<bool> Animate(
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

    private static Flow ToLayoutBounds(
        this Flow flow, Rect value, double start = 0, double end = 1)
    {
        return flow.Add(
            new FlowProperty(
                flow.View,
                AbsoluteLayout.LayoutBoundsProperty,
                value,
                () =>
                {
                    var start = (Rect)flow.View.GetValue(AbsoluteLayout.LayoutBoundsProperty);
                    return t => new Rect(
                        start.X + t * (value.X - start.X),
                        start.Y + t * (value.Y - start.Y),
                        start.Width + t * (value.Width - start.Width),
                        start.Height + t * (value.Height - start.Height));
                },
                start,
                end));
    }

    private static Flow ToThickness(
        this Flow flow, BindableProperty property, Thickness value, double start = 0, double end = 1)
    {
#pragma warning disable IDE0046 // Convert to conditional expression
        if (property.ReturnType != typeof(Thickness))
            throw new ArgumentException("Property flow error. Property must be of type Thickness", nameof(property));
#pragma warning restore IDE0046 // Convert to conditional expression

        return flow.Add(
            new FlowProperty(
                flow.View,
                property,
                value,
                () =>
                {
                    var start = (Thickness)flow.View.GetValue(property);
                    return t => new Thickness(
                        start.Left + t * (value.Left - start.Left),
                        start.Top + t * (value.Top - start.Top),
                        start.Right + t * (value.Right - start.Right),
                        start.Bottom + t * (value.Bottom - start.Bottom));
                },
                start,
                end));
    }

    private static Flow ToColor(
        this Flow flow, BindableProperty property, Color color, double start = 0, double end = 1)
    {
#pragma warning disable IDE0046 // Convert to conditional expression
        if (property.ReturnType != typeof(Color))
            throw new ArgumentException("Property flow error. Property must be of type Color", nameof(property));
#pragma warning restore IDE0046 // Convert to conditional expression

        return flow.Add(
            new FlowProperty(
            flow.View,
            property,
            color,
            () =>
            {
                var start = (Color)flow.View.GetValue(property);
                return t => Color.FromRgba(
                    start.Red + t * (color.Red - start.Red),
                    start.Green + t * (color.Green - start.Green),
                    start.Blue + t * (color.Blue - start.Blue),
                    start.Alpha + t * (color.Alpha - start.Alpha));
            },
            start,
            end));
    }

    private static Flow ToDouble(
        this Flow flow, BindableProperty property, double value, double start = 0, double end = 1)
    {
        if (property.ReturnType != typeof(double))
            throw new ArgumentException("Property flow error. Property must be of type double", nameof(property));

        var a = (double)flow.View.GetValue(property);

        return flow.Add(
            new FlowProperty(
                flow.View,
                property,
                value,
                () =>
                {
                    var start = (double)flow.View.GetValue(property);
                    return t => start + t * (value - start);
                },
                start,
                end));
    }

    private static bool IsNumber(this object value)
    {
        return value is sbyte
                or byte
                or short
                or ushort
                or int
                or uint
                or long
                or ulong
                or float
                or double
                or decimal;
    }
}
