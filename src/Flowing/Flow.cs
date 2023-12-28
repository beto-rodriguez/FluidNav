using System.Collections;

namespace FluidNav.Flowing;

public class Flow(View view) : IEnumerable<FlowProperty>
{
    private readonly List<FlowProperty> _flowProperties = [];

    public View View { get; } = view;

    public Flow Add(FlowProperty flowProperty)
    {
        _flowProperties.Add(flowProperty);
        return this;
    }

    public IEnumerator<FlowProperty> GetEnumerator() => _flowProperties.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => _flowProperties.GetEnumerator();
}
