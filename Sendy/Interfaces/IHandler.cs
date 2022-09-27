namespace Sendy.Interfaces;

public interface IHandler<in TSetRequest, out TSetResponse> where TSetRequest : IRequest<TSetResponse>
{
    public TSetResponse Handle(TSetRequest request);
}

public interface IHandler<TSetRequest> where TSetRequest : IRequest
{
   public void Handle(TSetRequest request);
}