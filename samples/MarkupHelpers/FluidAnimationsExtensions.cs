namespace Sample.MarkupHelpers;

public static class FluidAnimationsExtensions
{
    public static PropertyState StateForDoubleProperty(
        this VisualElement element, BindableProperty property, double value)
    {
        if (property.ReturnType != typeof(double))
            throw new ArgumentException("Property must be of type double", nameof(property));

        return new PropertyState(element, property, value, () =>
        {
            var start = (double)element.GetValue(property);
            return t => start + t * (value - start);
        });
    }

    public static PropertyState StateForLayoutBounds(
        this VisualElement element, double x, double y)
    {
        return new PropertyState(
            element,
            AbsoluteLayout.LayoutBoundsProperty,
            new Rect(x, y, AbsoluteLayout.AutoSize, AbsoluteLayout.AutoSize),
            () =>
            {
                var start = (Rect)element.GetValue(AbsoluteLayout.LayoutBoundsProperty);
                return t => new Rect(
                    start.X + t * (x - start.X),
                    start.Y + t * (y - start.Y),
                    AbsoluteLayout.AutoSize,
                    AbsoluteLayout.AutoSize);
            });
    }

    public static View SetState(this View view, IEnumerable<PropertyState> states)
    {
        foreach (var propertyState in states)
            propertyState.Setter(propertyState.TargetValue);
        return view;
    }

    public static Animation GetAnimation(this IEnumerable<PropertyState> states)
    {
        var animation = new Animation();
        foreach (var state in states) animation.Add(0, 1, state.GetAnimation());
        return animation;
    }

    /// <summary>
    /// Starts the animation and returns a task that completes when the animation ends.
    /// </summary>
    /// <param name="animation">The animation.</param>
    /// <param name="owner">Identifies the owner of the animation. This can be the visual element on which the animation is applied, or another visual element, such as the page</param>
    /// <param name="duration">The duration in milliseconds.</param>
    /// <param name="easing">The easing function.</param>
    /// <param name="fps">Frames per second, default is 60.</param>
    /// <param name="animationName">The animation identifier name, by default a new Guid is used.</param>
    /// <returns></returns>
    public static Task<bool> Start(
        this Animation animation,
        VisualElement owner, uint duration = 500, Easing? easing = null, uint fps = 60, string? animationName = null)
    {
        animationName ??= $"animation-{Guid.NewGuid()}";
        easing ??= Easing.CubicOut;

        var taskCompletionSource = new TaskCompletionSource<bool>();

        animation.Commit(
            owner, animationName, 1000 / fps, duration, easing, (v, c) => taskCompletionSource.SetResult(c));

        return taskCompletionSource.Task;
    }
}
