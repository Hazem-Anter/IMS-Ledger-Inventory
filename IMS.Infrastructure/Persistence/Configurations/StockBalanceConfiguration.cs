
using IMS.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IMS.Infrastructure.Persistence.Configurations
{
    public class StockBalanceConfiguration : IEntityTypeConfiguration<StockBalance>
    {
        public void Configure(EntityTypeBuilder<StockBalance> builder)
        {

            builder.ToTable("StockBalances");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.QuantityOnHand)
                .IsRequired();

            builder.HasOne(x => x.Product)
                .WithMany()
                .HasForeignKey(x => x.ProductId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.Warehouse)
                .WithMany()
                .HasForeignKey(x => x.WarehouseId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Property(x => x.LocationId).IsRequired(false);

            builder.HasOne(x => x.Location)
                .WithMany()
                .HasForeignKey(x => x.LocationId)
                .OnDelete(DeleteBehavior.Restrict);

            // Unique "current stock row" per Product + Warehouse + Location
            builder.HasIndex(x => new { x.ProductId, x.WarehouseId, x.LocationId })
                .IsUnique();

            // Concurrency token
            builder.Property(x => x.RowVersion)
                .IsRowVersion()
                .IsConcurrencyToken();

        }
    }
}
