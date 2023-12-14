using CommunityToolkit.Maui.Markup;
using FluidNav;
using FluidNav.Flowing;
using Sample.Data;

namespace Sample.Views;

public class Playlist(RouteParams routeParams, DataAccessLayer dal) : FluidView
{
    private readonly RouteParams _routeParams = routeParams;
    private readonly DataAccessLayer _dal = dal;
    private PlaylistTransitionView _transitionView = null!;

    public override View GetView()
    {
        //return new PlaylistTransitionView().FlowToResult(v => v.CardViewFlow);

        return new VerticalStackLayout()
        {
            new PlaylistTransitionView().Ref(out _transitionView),
            new Label()
                .Text("this is something else...")
        };
    }

    public override void OnEnter()
    {
        var idParam = _routeParams["id"];
        var id = int.Parse(idParam);

        BindingContext = _dal.Users.First(u => u.Id == id);

        _ = _transitionView.FlowToResult(v => v.CardViewFlow);
        _ = _transitionView._root.Flow(v => v.Flows().ToDouble(HeightRequestProperty, 650));
        _ = _transitionView._descriptionLabel.Flow(v => v.Flows().ToDouble(OpacityProperty, 1));
    }

    public override void OnLeave()
    {

    }
}
