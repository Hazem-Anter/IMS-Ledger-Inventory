
using IMS.Application.Common.Behaviors;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace IMS.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplicationService(this IServiceCollection services)
        {
            // Register MediatR services from the current assembly
            // current assembly is IMS.Application which contains the handlers and requests  
            services.AddMediatR(cfg 
                => cfg.RegisterServicesFromAssembly(typeof(DependencyInjection).Assembly));

            // Register pipeline behaviors 
            // CachingBehavior for caching requests that implement ICacheableQuery
            // This behavior will intercept requests and handle caching logic
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(CachingBehavior<,>));

            return services;
        }
    }
}
