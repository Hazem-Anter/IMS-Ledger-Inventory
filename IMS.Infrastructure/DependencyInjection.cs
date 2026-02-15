
using IMS.Application.Abstractions.Auth;
using IMS.Application.Abstractions.Caching;
using IMS.Application.Abstractions.Persistence;
using IMS.Application.Abstractions.Read;
using IMS.Infrastructure.Caching;
using IMS.Infrastructure.Identity;
using IMS.Infrastructure.Identity.Token;
using IMS.Infrastructure.Persistence;
using IMS.Infrastructure.Persistence.Interceptors;
using IMS.Infrastructure.Read;
using IMS.Infrastructure.Repositories;
using Microsoft.AspNetCore.Identity;
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


            // Register AppDbContext with SQL Server provider
            // and add the AuditSaveChangesInterceptor to track changes for auditing purposes.
            var connectionString = configuration.GetConnectionString("ImsConnection");

            services.AddDbContext<AppDbContext>((sp, options) =>
            {
                options.UseSqlServer(configuration.GetConnectionString("ImsConnection"));
                options.AddInterceptors(sp.GetRequiredService<AuditSaveChangesInterceptor>());
            });



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


            // Register AuthDbContext and Identity services for authentication and authorization
            services.AddDbContext<AuthDbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("AuthConnection")));

            // Configure Identity with custom options and add support for roles
            // You can customize the password requirements and lockout settings as needed
            // For example, here we set a minimum password length and require digits
            // , but you can adjust these settings based on your security requirements.
            services.AddIdentityCore<ApplicationUser>(options =>
            {
                options.Password.RequiredLength = 6;
                options.Password.RequireDigit = true;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;

                // optional lockout basics:
                options.Lockout.AllowedForNewUsers = true;
                options.Lockout.MaxFailedAccessAttempts = 5;
            })
            .AddRoles<IdentityRole<int>>()  // Add support for roles with integer keys
            .AddEntityFrameworkStores<AuthDbContext>()  // Use AuthDbContext for Identity stores. Ex : user and role management, and other related tables. 
            .AddSignInManager();    // Add SignInManager for handling user sign-in operations. Ex : login, logout, and other related functionalities.


            // Register JWT token service and configure JWT options from configuration
            services.Configure<JwtOptions>(configuration.GetSection("Jwt"));
            services.AddSingleton<IJwtTokenService, JwtTokenService>();

            // Register authentication service that will handle user registration, login, and other auth-related operations
            services.AddScoped<IAuthService, AuthService>();

            // Register current user service to access information about the currently authenticated user in the application
            services.AddHttpContextAccessor();
            services.AddScoped<ICurrentUserService, CurrentUserService>();
            services.AddScoped<AuditSaveChangesInterceptor>();

            // Register other read services as needed
            services.AddScoped<IProductReadService, ProductReadService>();

            // Register warehouse read service for retrieving warehouse-related data.
            services.AddScoped<IWarehouseReadService, WarehouseReadService>();

            // Register location read service for retrieving location-related data.
            services.AddScoped<ILocationReadService, LocationReadService>();

            // Register dashboard read service for retrieving data related to the dashboard,
            // such as statistics, summaries, and other relevant information for display on the dashboard.
            services.AddScoped<IDashboardReadService, DashboardReadService>();

            // Register identity initialization service to handle the setup of initial roles and admin user during application startup.
            services.AddScoped<IIdentityInitializationService, IdentityInitializationService>();

            // Register user and role administration services for managing users and roles in the application.
            services.AddScoped<IUserAdminService, UserAdminService>();
            services.AddScoped<IRoleAdminService, RoleAdminService>();


            return services;
        }
    }
}
