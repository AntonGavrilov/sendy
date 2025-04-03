using System.Reflection;

namespace Sendy.Interfaces;

public interface ISendy
{
   public TResponse Send<TResponse>(IRequest<TResponse> request);

   public void Send(IRequest request);

   public Task<TResponse> SendAsync<TResponse>(IAsyncRequest<TResponse> request);

   public Task SendAsync(IAsyncRequest request);

}