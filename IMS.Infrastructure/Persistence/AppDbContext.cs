
using IMS.Domain.Common;
using IMS.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

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


            // GLOBAL auditing config for all AuditableEntity descendants
            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                var clrType = entityType.ClrType;
                if (clrType == null) continue;

                if (typeof(AuditableEntity).IsAssignableFrom(clrType))
                {
                    // CreatedAt: default from DB (UTC), generated on add, never updated
                    modelBuilder.Entity(clrType)
                        .Property<DateTime>(nameof(AuditableEntity.CreatedAt))
                        .HasDefaultValueSql("SYSUTCDATETIME()")
                        .ValueGeneratedOnAdd();

                    modelBuilder.Entity(clrType)
                        .Property<DateTime>(nameof(AuditableEntity.CreatedAt))
                        .Metadata.SetAfterSaveBehavior(PropertySaveBehavior.Ignore);

                    // UpdatedAt: default from DB (UTC) on insert (real updates handled in SaveChanges)
                    modelBuilder.Entity(clrType)
                        .Property<DateTime?>(nameof(AuditableEntity.UpdatedAt))
                        .HasDefaultValueSql("SYSUTCDATETIME()")
                        .ValueGeneratedOnAdd();
                }
            }
        }

        // Override SaveChangesAsync to automatically set auditing properties
        // for entities that inherit from AuditableEntity
        // this method is for UpdateAt because CreatedAt is handled by the DB default value on insert
        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            var utcNow = DateTime.UtcNow;

            foreach (var entry in ChangeTracker.Entries<AuditableEntity>())
            {
                if (entry.State == EntityState.Added)
                {
                    entry.Entity.CreatedAt = utcNow;
                    entry.Entity.UpdatedAt = utcNow;
                }
                else if (entry.State == EntityState.Modified)
                {
                    entry.Entity.UpdatedAt = utcNow;

                    // extra safety: never allow CreatedAt to be modified
                    entry.Property(x => x.CreatedAt).IsModified = false;
                }
            }

            return base.SaveChangesAsync(cancellationToken);
        }


    }
}
