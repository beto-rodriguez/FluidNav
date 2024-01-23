
namespace FluidNav;

public abstract class ModalContent : FluidView
{
    public ModalView ModalView { get; internal set; } = null!;

    public virtual void CloseModal() => FlowNavigation.Current.CloseModal(ModalView);
}

public abstract class ModalContent<TResponse> : ModalContent
{
    private bool _hasResponse = false;
    private TaskCompletionSource<TResponse> _responseTaskCompletionSource = null!;

    public TaskCompletionSource<TResponse> ResponseTaskCompletionSource
    {
        get => _responseTaskCompletionSource;
        internal set
        {
            _responseTaskCompletionSource = value;
            _hasResponse = false;
        }
    }

    public void SetModalResponse(object response) => SetModalResponse((TResponse)response);

    public void SetModalResponse(TResponse response)
    {
        if (_hasResponse) return;

        _hasResponse = true;
        ResponseTaskCompletionSource.SetResult(response);
        FlowNavigation.Current.CloseModal(ModalView);
    }

    public override void CloseModal()
    {
        ResponseTaskCompletionSource.SetResult(default!);
        base.CloseModal();
    }
}
