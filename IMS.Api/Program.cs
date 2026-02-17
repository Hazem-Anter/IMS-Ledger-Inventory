
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
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Security.Claims;
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


            // Configure Swagger to include JWT authentication support.
            // This allows us to test authenticated endpoints directly from the Swagger UI by providing a JWT token.
            builder.Services.AddSwaggerGen(c =>
            {
                // 1) Define the Swagger document with a title and version.
                // This is the basic information about our API that will be displayed in the Swagger UI.
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "IMS API",
                    Version = "v1"
                });

                // 2) Add a security definition for Bearer authentication.
                // This tells Swagger that our API uses JWT Bearer tokens for authentication and how to provide them in the UI.
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "Enter your JWT token like: Bearer {token}"
                });

                // 3) Add a security requirement that applies the Bearer authentication scheme to all endpoints.
                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        Array.Empty<string>()
                    }
                });

                c.OperationFilter<Swagger.SetupKeyHeaderOperationFilter>();

            });


            // Add Infrastructure Services 
            builder.Services.AddInfrastructureServices(builder.Configuration);
            // Add Application Services
            builder.Services.AddApplicationService();

            // Register TimeProvider as a singleton service in the dependency injection container.
            // This allows us to inject TimeProvider into other services or controllers that require time-related functionality.
            // solve the problem of time management in the application.
            builder.Services.AddSingleton(TimeProvider.System);

            // Configure Health Checks for SQL Server databases
            // This section adds health checks to the application to monitor the health of the SQL Server databases used by the application.
            builder.Services.AddHealthChecks()
                .AddSqlServer(
                    connectionString: builder.Configuration.GetConnectionString("ImsConnection")!,
                    name: "ims-db",
                    tags: new[] { "ready" })
                .AddSqlServer(
                    connectionString: builder.Configuration.GetConnectionString("AuthConnection")!,
                    name: "auth-db",
                    tags: new[] { "ready" });



            // ##########################################################################################
            // Configure JWT Authentication  
            // This section sets up JWT authentication for the application.
            // It reads JWT configuration from the appsettings.json file,
            // validates incoming JWT tokens, and ensures that only authenticated users can access protected endpoints.
            var jwt = builder.Configuration.GetSection("Jwt").Get<JwtOptions>()
                        ?? throw new InvalidOperationException("Jwt configuration is missing.");


            // Configure the authentication services to use JWT Bearer tokens.
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
                        IssuerSigningKey = new SymmetricSecurityKey(
                            Encoding.UTF8.GetBytes(jwt.Secret)),

                        ClockSkew = TimeSpan.FromSeconds(30)
                    };

                    // Add an event handler for token validation to check if the user is deactivated.
                    // This allows us to implement a custom deactivation mechanism
                    // where we can check if the user associated with the JWT token is still active (not deactivated)
                    // before allowing access to protected resources.
                    options.Events = new JwtBearerEvents
                    {
                        OnTokenValidated = async context =>
                        {
                            var userManager = context.HttpContext.RequestServices
                                .GetRequiredService<UserManager<ApplicationUser>>();

                            var userId = context.Principal?.FindFirstValue(ClaimTypes.NameIdentifier);

                            if (string.IsNullOrEmpty(userId))
                            {
                                context.Fail("Invalid token: missing user id.");
                                return;
                            }

                            var user = await userManager.FindByIdAsync(userId);

                            if (user == null)
                            {
                                context.Fail("User no longer exists.");
                                return;
                            }

                            // This is your existing deactivation mechanism
                            if (user.LockoutEnd.HasValue &&
                                user.LockoutEnd.Value > DateTimeOffset.UtcNow)
                            {
                                context.Fail("User is deactivated.");
                                return;
                            }
                        }
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

            //app.UseHttpsRedirection();
            if (!app.Environment.IsDevelopment())
            {
                app.UseHttpsRedirection();
            }


            app.UseAuthentication();
            app.UseAuthorization();


            // Configure health check endpoints for liveness and readiness probes.
            app.MapHealthChecks("/health/live", new Microsoft.AspNetCore.Diagnostics.HealthChecks.HealthCheckOptions
            {
                Predicate = _ => false // liveness: app is running
            });

            app.MapHealthChecks("/health/ready", new Microsoft.AspNetCore.Diagnostics.HealthChecks.HealthCheckOptions
            {
                Predicate = r => r.Tags.Contains("ready") // readiness: DB checks
            });


            app.MapControllers();


            // Auto-migrate databases (recommended only for Development / Docker demo)
            if (app.Configuration.GetValue<bool>("Database:AutoMigrate"))
            {
                using var scope = app.Services.CreateScope();

                var imsDb = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                await imsDb.Database.MigrateAsync();

                var authDb = scope.ServiceProvider.GetRequiredService<AuthDbContext>();
                await authDb.Database.MigrateAsync();
            }


            await app.RunAsync();
        }
    }
}
