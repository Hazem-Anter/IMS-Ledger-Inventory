
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

            return services;
        }
    }
}
