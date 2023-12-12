using FluidNav;

namespace Sample;

public partial class FluidHostPage : ContentPage, IFluidPage
{
    public FluidHostPage()
    {
        InitializeComponent();
    }

    public ContentPresenter Presenter => content;

    private void OnBackClicked(object sender, EventArgs e)
    {
        Fluid.MainView.GoBack();
    }
}
