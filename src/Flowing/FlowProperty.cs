namespace FluidNav.Flowing;

public class FlowProperty(
    BindableObject bindable,
    BindableProperty property,
    object targetValue,
    Func<Func<double, object>> transform,
    double start = 0,
    double end = 1)
{
    /// <summary>
    /// Gets the target value.
    /// </summary>
    public object TargetValue { get; } = targetValue;

    /// <summary>
    /// Sets the property to the given value.
    /// </summary>
    public Action<object> Setter { get; } = v => bindable.SetValue(property, v);

    /// <summary>
    /// Gets the animation start value.
    /// </summary>
    public double Start { get; } = start;

    /// <summary>
    /// Gets the animation end value.
    /// </summary>
    public double End { get; set; } = end;

    /// <summary>
    /// Gets a normalized animation [from 0 to 1] for the property state.
    /// </summary>
    /// <returns></returns>
    public Animation GetAnimation()
    {
        var currentTransform = transform();
        return new Animation(t => Setter(currentTransform(t)), 0, 1);
    }
}
