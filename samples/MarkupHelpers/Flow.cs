using System.Collections;

namespace Sample.MarkupHelpers;

public class Flow(VisualElement visual) : IEnumerable<FlowProperty>
{
    private readonly List<FlowProperty> _flowProperties = [];

    public VisualElement VisualElement { get; } = visual;

    public Flow Add(FlowProperty flowProperty)
    {
        _flowProperties.Add(flowProperty);
        return this;
    }

    public IEnumerator<FlowProperty> GetEnumerator()
    {
        return _flowProperties.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return _flowProperties.GetEnumerator();
    }
}
