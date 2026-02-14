
using IMS.Api.Middleware;
using IMS.Application;
using IMS.Application.Abstractions.Auth;
using IMS.Infrastructure;
using IMS.Infrastructure.Identity;
using IMS.Infrastructure.Persistence;
using IMS.Infrastructure.Persistence.Seeding;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace IMS.Api
{
    public class Program
    {
        // Entry point for the application.
        // Async because we are awaiting asynchronous operations during startup.
        // asynchronous like database seeding
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);



            // AddControllers with a global authorization filter. 
            // This ensures that all endpoints require authentication by default,
            // and we can override this behavior on specific controllers or actions if needed.
            builder.Services.AddControllers(options =>
            {
                var policy = new AuthorizationPolicyBuilder()        // Create a new authorization policy that requires authenticated users.
                    .RequireAuthenticatedUser()                      // This method adds a requirement to the policy that the user must be authenticated.
                    .Build();                                        // Build the policy, which compiles all the requirements into a single policy object.
                options.Filters.Add(new AuthorizeFilter(policy));    // Add the authorization filter to the MVC options, which applies the policy globally to all controllers and actions.
            });

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            // Add Infrastructure Services 
            builder.Services.AddInfrastructureServices(builder.Configuration);
            // Add Application Services
            builder.Services.AddApplicationService();

            // Register TimeProvider as a singleton service in the dependency injection container.
            // This allows us to inject TimeProvider into other services or controllers that require time-related functionality.
            // solve the problem of time management in the application.
            builder.Services.AddSingleton(TimeProvider.System);


            // ##########################################################################################
            // Configure JWT Authentication  
            // This section sets up JWT authentication for the application.
            // It reads JWT configuration from the appsettings.json file,
            // validates incoming JWT tokens, and ensures that only authenticated users can access protected endpoints.
            var jwt = builder.Configuration.GetSection("Jwt").Get<JwtOptions>()
                        ?? throw new InvalidOperationException("Jwt configuration is missing.");

            builder.Services
                .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,

                        ValidIssuer = jwt.Issuer,
                        ValidAudience = jwt.Audience,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwt.Secret)),

                        ClockSkew = TimeSpan.FromSeconds(30)
                    };
                });

            // ##########################################################################################


            // Register the global exception handling middleware as a transient service in the dependency injection container.
            builder.Services.AddTransient<GlobalExceptionMiddleware>();

            var app = builder.Build();

            /*
            // Seed Identity data (roles and users)
            using (var scope = app.Services.CreateScope())
            {
                var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole<int>>>();
                var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
                var config = scope.ServiceProvider.GetRequiredService<IConfiguration>();

                await IdentitySeeder.SeedAsync(roleManager, userManager, config);
            }
            */
            /*
            // Seed the database during development
            // This ensures that the database has initial data for testing and development purposes.
            if (app.Environment.IsDevelopment())
            {
                using var scope = app.Services.CreateScope();
                var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                await AppDbSeeder.SeedAsync(db);
            }
            */

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            // Add the global exception handling middleware to the request pipeline.
            app.UseMiddleware<GlobalExceptionMiddleware>();

            app.UseHttpsRedirection();

            app.UseAuthentication();
            app.UseAuthorization();


            app.MapControllers();

            await app.RunAsync();
        }
    }
}
