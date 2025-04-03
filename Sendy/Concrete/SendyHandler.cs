using Sendy.Interfaces;
using System.Reflection;

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

   public TResponse Send<TResponse>(IRequest<TResponse> request) 
   {
      if (request == null)
      {
         throw new ArgumentNullException(nameof(request));
      }

      var generic = typeof(IHandler<,>);

      var response = request.GetType().GetInterfaces()[0].GetGenericArguments()[0];

      Type[] typeArgs = { request.GetType(), response };

      Type constructed = generic.GetGenericTypeDefinition().MakeGenericType(typeArgs);

      var targetTypeList = _assembly
         .GetTypes()
         .Where(t => t.GetInterfaces()
         .Any(t => t == constructed)).ToArray();

      if (targetTypeList.Length == 0)
         throw new Exception($"There is no handlers of request '{request.GetType().Name}'");

      if (targetTypeList.Length > 1)
         throw new Exception($"There are several handlers of request '{request.GetType().Name}'");

      var handlerType = targetTypeList[0];

      var handler = _serviceProvider.GetService(targetTypeList[0]);
      var method = handlerType.GetMethod("Handle");

      var result = method?.Invoke(handler, new object[] { request });

      if(result == null)
      {
         return default;
      }

      return (TResponse)result;
   }

   public void Send(IRequest request)
   {
      if (request == null)
      {
         throw new ArgumentNullException(nameof(request));
      }

      var generic = typeof(IHandler<IRequest>);
      Type[] typeArgs = { request.GetType() };

      Type constructed = generic.GetGenericTypeDefinition().MakeGenericType(typeArgs);

      var targetTypeList = _assembly
         .GetTypes()
         .Where(t => t.GetInterfaces()
         .Any(t => t == constructed)).ToArray();

      if (targetTypeList.Length == 0)
         throw new Exception($"There is no handlers of request '{request.GetType().Name}'");

      if (targetTypeList.Length > 1)
         throw new Exception($"There are several handlers of request '{request.GetType().Name}'");

      var handlerType = targetTypeList[0];

      var handler = _serviceProvider.GetService(handlerType);
      var method = handlerType.GetMethod("Handle");


      method?.Invoke(handler, new object[] { request });
   }

   public async Task<TResponse> SendAsync<TResponse>(IAsyncRequest<TResponse> request) 
   {
      if (request == null)
      {
         throw new ArgumentNullException(nameof(request));
      }

      var generic = typeof(IAsyncHandler<,>);
      var response = request.GetType().GetInterfaces()[0].GetGenericArguments()[0];
      Type[] typeArgs = { request.GetType(), response };

      Type constructed = generic.GetGenericTypeDefinition().MakeGenericType(typeArgs);

      var targetTypeList = _assembly
         .GetTypes()
         .Where(t => t.GetInterfaces()
         .Any(t => t == constructed)).ToArray();

      if (targetTypeList.Length == 0)
         throw new ApplicationException($"There is no handlers of request '{request.GetType().Name}'");

      if (targetTypeList.Length > 1)
         throw new ApplicationException($"There are several handlers of request '{request.GetType().Name}'");

      var handlerType = targetTypeList[0];

      var handler = _serviceProvider.GetService(handlerType);
      var method = handlerType.GetMethod("Handle");

      if(method == null)
         throw new ApplicationException($"There are no Handle method in '{request.GetType().Name}'");

      var result = (Task<TResponse>)method.Invoke(handler, new object[] { request });

      if(result == null)
      {
         return default;
      }

      return await result;
   }

   public async Task SendAsync(IAsyncRequest request)
   {
      if (request == null)
      {
         throw new ArgumentNullException(nameof(request));
      }

      var generic = typeof(IAsyncHandler<IAsyncRequest>);
      Type[] typeArgs = { request.GetType() };

      Type constructed = generic.GetGenericTypeDefinition().MakeGenericType(typeArgs);

      var targetTypeList = _assembly
         .GetTypes()
         .Where(t => t.GetInterfaces()
         .Any(t => t == constructed)).ToArray();

      if (targetTypeList.Length == 0)
         throw new Exception($"There is no handlers of request '{request.GetType().Name}'");

      if (targetTypeList.Length > 1)
         throw new Exception($"There are several handlers of request '{request.GetType().Name}'");

      var handlerType = targetTypeList[0];

      var handler = _serviceProvider.GetService(handlerType);
      var method = handlerType.GetMethod("Handle");

      if (method == null)
         throw new ApplicationException($"There are no Handle method in '{request.GetType().Name}'");

      var task = (Task)method?.Invoke(handler, new object[] { request });

      await task;
   }

}