namespace Sendy.Interfaces;

public interface IHandler<in TRequest, TResponse> where TRequest : IRequest<TResponse>
{
   public TResponse Handle(TRequest request);
}

public interface IHandler<TRequest> where TRequest : IRequest
{
   public void Handle(TRequest request);   
}


public interface IAsyncHandler<in TRequest, TResponse> where TRequest : IAsyncRequest<TResponse>
{
   public Task<TResponse> Handle(TRequest request);
}

public interface IAsyncHandler<TRequest> where TRequest : IAsyncRequest
{
   public Task Handle(TRequest request);
}