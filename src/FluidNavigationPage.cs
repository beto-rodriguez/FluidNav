namespace FluidNav;

public class FluidNavigationPage(Page root) : NavigationPage(root)
{
    protected override bool OnBackButtonPressed()
    {
        _ = FlowNavigation.Current.GoBack();
        return true;
    }
}
