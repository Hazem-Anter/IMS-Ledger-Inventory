
using IMS.Domain.Common;

namespace IMS.Domain.Entities
{
    public class StockBalance : AuditableEntity
    {
        public int ProductId { get; private set; }
        public int WarehouseId { get; private set; }
        public int? LocationId { get; private set; }

        public int QuantityOnHand { get; private set; }

        // Concurrency token for EF Core optimistic concurrency control 
        public byte[] RowVersion { get; private set; } = default!;


        // navigation properties for EF Core
        public Product? Product { get; private set; }
        public Warehouse? Warehouse { get; private set; }
        public Location? Location { get; private set; }

        private StockBalance() { }

        public StockBalance(int productId, int warehouseId, int? locationId, int initialQuantity = 0) 
        {
            if(productId <= 0) throw new ArgumentException("ProductId must be valid.", nameof(productId));
            if(warehouseId <= 0) throw new ArgumentException("WarehouseId must be valid.", nameof(warehouseId));
            if(initialQuantity < 0) throw new ArgumentException("Initial quantity cannot be negative.", nameof(initialQuantity));

            ProductId = productId;
            WarehouseId = warehouseId;
            LocationId = locationId;
            QuantityOnHand = initialQuantity;
        }

        // Apply a delta to the stock balance (+ for IN, - for OUT) with business rule enforcement 
        public void ApplyDelta(int delta)
        {
            // Business rule (strict mode): don't allow negative stock
            var newQty = QuantityOnHand + delta;
            if (newQty < 0)
                throw new InvalidOperationException("Insufficient stock.");

            QuantityOnHand = newQty;
        }
    }
}
