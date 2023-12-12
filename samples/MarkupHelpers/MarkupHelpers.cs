using CommunityToolkit.Maui.Markup;
using Microsoft.Maui.Controls.Shapes;

namespace Sample.MarkupHelpers;

public static class MarkupHelpers
{
    public static Border Content(this Border border, View view)
    {
        border.Content = view;
        return border;
    }

    public static TView Children<TView>(this TView view, params View[] children)
        where TView : Layout
    {
        foreach (var child in children) view.Children.Add(child);
        return view;
    }

    public static TVisualElement Style<TVisualElement>(
        this TVisualElement element,
        Action<Style<TVisualElement>> styleBuilder)
            where TVisualElement : VisualElement
    {
        var style = new Style<TVisualElement>();
        styleBuilder(style);
        return element.Style(style);
    }
}


public static class Shapes
{
    public static IShape RoundRectangle(double cornerRadius)
    {
        return new RoundRectangle { CornerRadius = new(cornerRadius) };
    }
}

public static class Shadows
{
    public static Shadow Small()
    {
        return new Shadow
        {
            Radius = 8,
            Offset = new(0, 0),
            Opacity = 0.1f
        };
    }
}

