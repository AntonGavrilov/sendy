namespace Sendy.Interfaces
{
   public interface IAsyncHandler<in TRequest, TResponse> where TRequest : IAsyncRequest<TResponse>
   {
      public Task<TResponse> Handle(TRequest request);
   }

   public interface IAsyncHandler<TRequest> where TRequest : IAsyncRequest
   {
      public Task Handle(TRequest request);
   }
}
