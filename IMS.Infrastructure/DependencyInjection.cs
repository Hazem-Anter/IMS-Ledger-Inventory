
using IMS.Application.Abstractions.Caching;
using IMS.Application.Abstractions.Persistence;
using IMS.Application.Abstractions.Read;
using IMS.Infrastructure.Caching;
using IMS.Infrastructure.Persistence;
using IMS.Infrastructure.Read;
using IMS.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace IMS.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
        {
            // Here you can register infrastructure services, e.g., DbContext, repositories, etc.


            // Register AppDbContext with SQL Server provider using connection string from configuration
            var connectionString = configuration.GetConnectionString("DefaultConnection");

            services.AddDbContext<AppDbContext>(options =>
                 options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));


            // Register generic repository and unit of work
            services.AddScoped(typeof(IRepository<>), typeof(EfRepository<>));
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            // Register read services
            services.AddScoped<IStockReadService, StockReadService>();

            // Register caching services
            services.AddMemoryCache();
            services.AddScoped<ICacheService, MemoryCacheService>();

            // Register cache versioning service for cache invalidation 
            services.AddScoped<ICacheVersionService, MemoryCacheVersionService>();


            return services;
        }
    }
}
