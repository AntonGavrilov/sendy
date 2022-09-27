using System.Reflection;

namespace Sendy.Interfaces;

public interface ISendy
{
    public TResponse Send<TResponse>(IRequest<TResponse> request) where TResponse : class;

    public void Send(IRequest request);

}