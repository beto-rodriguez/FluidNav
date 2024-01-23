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
    /// Adds the given view to the root layout.
    /// </summary>
    /// <param name="view">The view.</param>
    void AddToRoot(View view);

    /// <summary>
    /// Removes the given view from the root layout.
    /// </summary>
    /// <param name="view"></param>
    void RemoveFromRoot(View view);

    /// <summary>
    /// Adds the given view to the presenter.
    /// </summary>
    /// <param name="view"></param>
    void AddToPresenter(View view);

    /// <summary>
    /// Removes the given view from the presenter.
    /// </summary>
    /// <param name="view"></param>
    void RemoveFromPresenter(View view);
}
