namespace FluidNav;

/// <summary>
/// Defines a fluid page.
/// </summary>
public interface IFluidHost
{
    /// <summary>
    /// Gets the width.
    /// </summary>
    double Width { get; }

    /// <summary>
    /// Gets the height.
    /// </summary>
    double Height { get; }

    /// <summary>
    /// Shows the specified view.
    /// </summary>
    void ShowView(View view);
}
