using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Sendy.Concrete;
using Sendy.Interfaces;

namespace Sendy.Registration;

public static class ServiceRegister
{
   public static IServiceCollection AddSendy(this IServiceCollection services, Assembly assembly)
   {
      services.AddScoped<ISendy, SendyHandler>(sp => new SendyHandler(assembly, sp));


      var interfaces = assembly.
         GetTypes()
         .Where
         (type =>
            type
            .GetInterfaces()
            .Any(t => t.IsGenericType && (t.GetGenericTypeDefinition() == typeof(IRequest<>)))).ToList();

      foreach (var request in interfaces)
      {
         var generic = typeof(IHandler<,>);
         var response = request.GetInterfaces()[0].GetGenericArguments()[0];
         Type[] typeArgs = { request, response };

         Type constructed = generic.GetGenericTypeDefinition().MakeGenericType(typeArgs);
         var targetTypeList =
             assembly
             .GetTypes()
             .Where(t => t.GetInterfaces()
                 .Any(t => t == constructed)).ToList();

         if (targetTypeList.Count == 0)
            throw new Exception($"There is no handlers of request '{request.Name}'");

         if (targetTypeList.Count > 1)
            throw new Exception($"There are several handlers of request '{request.Name}'");

         services.AddScoped(targetTypeList[0]);

      } 

      interfaces = assembly.
         GetTypes()
         .Where
         (type =>
            type
            .GetInterfaces()
            .Any(t => (t == typeof(IRequest)))).ToList();

      foreach (var request in interfaces)
      {
         var generic = typeof(IHandler<IRequest>);
         Type[] typeArgs = { request };

         Type constructed = generic.GetGenericTypeDefinition().MakeGenericType(typeArgs);
         var targetTypeList =
             assembly
             .GetTypes()
             .Where(t => t.GetInterfaces()
                 .Any(t => t == constructed)).ToList();

         if (targetTypeList.Count == 0)
            throw new Exception($"There is no handlers of request '{request.Name}'");

         if (targetTypeList.Count > 1)
            throw new Exception($"There are several handlers of request '{request.Name}'");

         services.AddScoped(targetTypeList[0]);

      }

      interfaces = assembly.
          GetTypes()
          .Where
          (type =>
             type
             .GetInterfaces()
             .Any(t => t.IsGenericType && (t.GetGenericTypeDefinition() == typeof(IAsyncRequest<>)))).ToList();

      foreach (var request in interfaces)
      {
         var generic = typeof(IAsyncHandler<,>);
         var response = request.GetInterfaces()[0].GetGenericArguments()[0];
         Type[] typeArgs = { request, response };

         Type constructed = generic.GetGenericTypeDefinition().MakeGenericType(typeArgs);
         var targetTypeList =
             assembly
             .GetTypes()
             .Where(t => t.GetInterfaces()
                 .Any(t => t == constructed)).ToList();

         if (targetTypeList.Count == 0)
            throw new Exception($"There is no handlers of request '{request.Name}'");

         if (targetTypeList.Count > 1)
            throw new Exception($"There are several handlers of request '{request.Name}'");

         services.AddScoped(targetTypeList[0]);

      }

      interfaces = assembly.
         GetTypes()
         .Where
         (type =>
            type
            .GetInterfaces()
            .Any(t => (t == typeof(IAsyncRequest)))).ToList();

      foreach (var request in interfaces)
      {
         var generic = typeof(IAsyncHandler<IAsyncRequest>);
         Type[] typeArgs = { request };

         Type constructed = generic.GetGenericTypeDefinition().MakeGenericType(typeArgs);
         var targetTypeList =
             assembly
             .GetTypes()
             .Where(t => t.GetInterfaces()
                 .Any(t => t == constructed)).ToList();

         if (targetTypeList.Count == 0)
            throw new Exception($"There is no handlers of request '{request.Name}'");

         if (targetTypeList.Count > 1)
            throw new Exception($"There are several handlers of request '{request.Name}'");

         services.AddScoped(targetTypeList[0]);

      }
      return services;
   }
}