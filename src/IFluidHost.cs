namespace FluidNav;

/// <summary>
/// Defines a fluid page.
/// </summary>
public interface IFluidHost
{
    /// <summary>
    /// Shows the specified view, null to clear it.
    /// </summary>
    void ShowView(View view);
}
