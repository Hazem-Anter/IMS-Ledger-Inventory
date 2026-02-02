
using IMS.Application.Abstractions.Persistence;
using IMS.Infrastructure.Persistence;
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

            /////////////////////////////////////////////////////////////////////
            // Register AppDbContext with SQL Server provider using connection string from configuration
            /////////////////////////////////////////////////////////////////////
            var connectionString = configuration.GetConnectionString("DefaultConnection");

            services.AddDbContext<AppDbContext>(options =>
                 options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));
            /////////////////////////////////////////////////////////////////////

            /////////////////////////////////////////////////////////////////////
            // Register generic repository and unit of work
            /////////////////////////////////////////////////////////////////////
            services.AddScoped(typeof(IRepository<>), typeof(EfRepository<>));
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            /////////////////////////////////////////////////////////////////////


            return services;
        }
    }
}
