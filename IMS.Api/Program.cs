
using IMS.Application;
using IMS.Infrastructure;
using IMS.Infrastructure.Persistence;
using IMS.Infrastructure.Persistence.Seeding;

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

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            // Add Infrastructure Services 
            builder.Services.AddInfrastructureServices(builder.Configuration);
            // Add Application Services
            builder.Services.AddApplicationService();

            var app = builder.Build();

            // Seed the database during development
            // This ensures that the database has initial data for testing and development purposes.
            if (app.Environment.IsDevelopment())
            {
                using var scope = app.Services.CreateScope();
                var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                await AppDbSeeder.SeedAsync(db);
            }

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            await app.RunAsync();
        }
    }
}
