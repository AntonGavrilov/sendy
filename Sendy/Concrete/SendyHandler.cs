using Sendy.Interfaces;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;


namespace Sendy.Concrete;

public class SendyHandler : ISendy
{
   private Assembly _assembly;
   private readonly IServiceProvider _serviceProvider;
   public SendyHandler(Assembly assembly, IServiceProvider serviceProvider)
   {
      _assembly = assembly;
      _serviceProvider = serviceProvider;
   }

   public TResponse Send<TResponse>(IRequest<TResponse> request) where TResponse : class
   {
      var generic = typeof(IHandler<,>);
      var response = request.GetType().GetInterfaces()[0].GetGenericArguments()[0];
      Type[] typeArgs = { request.GetType(), response };

      Type constructed = generic.GetGenericTypeDefinition().MakeGenericType(typeArgs);

      var targetTypeList = _assembly
         .GetTypes()
         .Where(t => t.GetInterfaces()
         .Any(t => t == constructed)).ToList();

      var handler = _serviceProvider.GetService(targetTypeList[0]);
      var method = targetTypeList[0].GetMethod("Handle");

      var result = method?.Invoke(handler, new object?[] { request });

      if(result == null)
      {
         return null;
      }

      return (TResponse)result;
   }

   public void Send(IRequest request)
   {
      var generic = typeof(IHandler<IRequest>);
      Type[] typeArgs = { request.GetType() };

      Type constructed = generic.GetGenericTypeDefinition().MakeGenericType(typeArgs);

      var targetTypeList = _assembly
         .GetTypes()
         .Where(t => t.GetInterfaces()
         .Any(t => t == constructed)).ToList();

      var handler = _serviceProvider.GetService(targetTypeList[0]);
      var method = targetTypeList[0].GetMethod("Handle");


      method?.Invoke(handler, new object?[] { request });
   }
}