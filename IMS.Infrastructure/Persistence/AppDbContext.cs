
using IMS.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace IMS.Infrastructure.Persistence
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Product> Products => Set<Product>();
        public DbSet<Warehouse> Warehouses => Set<Warehouse>();
        public DbSet<Location> Locations => Set<Location>();
        public DbSet<StockBalance> StockBalances => Set<StockBalance>();
        public DbSet<StockTransaction> StockTransactions => Set<StockTransaction>();

        // Configure entity mappings using Fluent API 
        // This method is called when the model for a derived context has been initialized
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Call the base method to ensure any configurations in the base class are applied
            base.OnModelCreating(modelBuilder);


            // Apply all configurations from the current assembly
            // This will automatically register all IEntityTypeConfiguration implementations
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
        }

    }
}
