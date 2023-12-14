using FluidNav;

namespace Sample;

public partial class App : Application
{
    public App(IServiceProvider services)
    {
        InitializeComponent();
        MainPage = new FluidNavigationPage(services.GetFluidHost());
    }
}
