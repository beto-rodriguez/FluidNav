var template =
@"namespace FluidNav;

public static partial class ResponsiveExtensions
{
    {content}
}";

string[] defaultBreakpoints = ["Sm", "Md", "Lg", "Xl", "Xxl"];
string[] defaultConstants = ["1", "2", "3", "4", "5"];

Extension[] extensions =
[
    new("Padding", defaultConstants, defaultBreakpoints,
        [
            "public static T {name}_{breakpoint}_{constantName}<T>(this T view) where T : View => On{breakpoint}(view, v => view.Margin = new Thickness({constantValue}));",
            "public static T {name}_{breakpoint}_Start_{constantName}<T>(this T view) where T : View => On{breakpoint}(view, v => view.Margin = new Thickness({constantValue}, 0, 0, 0));",
            "public static T {name}_{breakpoint}_End_{constantName}<T>(this T view) where T : View => On{breakpoint}(view, v => view.Margin = new Thickness(0, 0, {constantValue}, 0));",
            "public static T {name}_{breakpoint}_Top_{constantName}<T>(this T view) where T : View => On{breakpoint}(view, v => view.Margin = new Thickness(0, 0, {constantValue}, 0));",
            "public static T {name}_{breakpoint}_Bottom_{constantName}<T>(this T view) where T : View => On{breakpoint}(view, v => view.Margin = new Thickness(0, 0, {constantValue}, 0));"
        ])
];



Console.WriteLine("Hello, World!");
