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
    /// Gets the presenter.
    /// </summary>
    View Presenter { get; }

    /// <summary>
    /// Gets the active breakpoint.
    /// </summary>
    BreakPoint ActiveBreakpoint { get; }

    /// <summary>
    /// Called when the breakpoint changes.
    /// </summary>
    event Action BreakpointChanged;

    /// <summary>
    /// Shows the specified view.
    /// </summary>
    void ShowView(View view);
}
