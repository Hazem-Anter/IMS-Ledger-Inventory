
using FluentValidation;
using IMS.Application.Common.Behaviors;
using IMS.Application.Features.Inventory.Commands.ReceiveStock;
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

            // Register LoggingBehavior for logging requests and responses
            // This behavior will intercept requests and log relevant information
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(LoggingBehavior<,>));


            // Register FluentValidation validators from the assembly containing ReceiveStockCommand,
            // Which is the IMS.Application assembly. This will automatically register all validators defined in that assembly.
            services.AddValidatorsFromAssembly(typeof(DependencyInjection).Assembly);
            // Register ValidationBehavior for validating requests using FluentValidation
            // This behavior will intercept requests and validate them before they reach the handlers
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));

            // Register pipeline behaviors 
            // CachingBehavior for caching requests that implement ICacheableQuery
            // This behavior will intercept requests and handle caching logic
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(CachingBehavior<,>));

            // CacheVersionInvalidationBehavior for invalidating cache versions on commands
            // that implement ICacheVersionInvalidator
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(CacheVersionInvalidationBehavior<,>));

            // CacheInvalidationBehavior for invalidating cache on commands that implement ICacheInvalidator
            // This behavior will intercept requests and handle cache invalidation logic
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(CacheInvalidationBehavior<,>));


            return services;
        }
    }
}
