namespace FluidNav;

public static class LayoutExtensions
{
    public static T Ref<T>(this T view, out T reference) where T : View => reference = view;

    public static Grid Rows(this Grid grid, RowDefinitionCollection rows)
    {
        grid.RowDefinitions = rows;
        return grid;
    }

    public static Grid Columns(this Grid grid, ColumnDefinitionCollection columns)
    {
        grid.ColumnDefinitions = columns;
        return grid;
    }

    public static T Children<T>(this T layout, params View[] children) where T : Layout
    {
        foreach (var child in children) layout.Children.Add(child);
        return layout;
    }

    public static T MaxWidth<T>(this T view, double width) where T : View
    {
        view.MaximumWidthRequest = width;
        return view;
    }
    public static T MaxHeight<T>(this T view, double height) where T : View
    {
        view.MaximumHeightRequest = height;
        return view;
    }

    public static T MinWidth<T>(this T view, double width) where T : View
    {
        view.MinimumWidthRequest = width;
        return view;
    }

}
