
using IMS.Domain.Common;
using IMS.Domain.Enum;

namespace IMS.Domain.Entities
{
    public class StockTransaction : AuditableEntity
    {
        public int ProductId { get; private set; }
        public int WarehouseId { get; private set; }
        public int LocationId { get; private set; }

        public StockTransactionType Type { get; private set; }

        // Always store the delta (+ for IN, - for OUT )
        public int QuantityDelta { get; private set; }

        // needed for inventory valuation methods like FIFO, LIFO, Average Costing
        public decimal? UnitCost { get; private set; }

        // reference to the source of the transaction (e.g., PurchaseOrder, SalesOrder, etc.)
        public string? ReferenceType { get; private set; }
        public string? ReferenceId { get; private set; }

        // navigation properties for EF Core 
        public Product? Product { get; private set; }
        public Warehouse? Warehouse { get; private set; }
        public Location? Location { get; private set; }

        private StockTransaction() { }
        private StockTransaction(
            int productId,
            int warehouseId,
            int? locationId,
            StockTransactionType type,
            int quantityDelta,
            decimal? unitCost,
            string? referenceType,
            string? referenceId)
        {
            if(productId <= 0) throw new ArgumentException("ProductId must be valid.", nameof(productId));
            if(warehouseId <= 0) throw new ArgumentException("WarehouseId must be valid.", nameof(warehouseId));
            if(quantityDelta == 0) throw new ArgumentException("QuantityDelta cannot be zero.", nameof(quantityDelta));

            ProductId = productId;
            WarehouseId = warehouseId;
            LocationId = locationId ?? 0;

            Type = type;
            QuantityDelta = quantityDelta;

            UnitCost = unitCost;
            ReferenceType = referenceType;
            ReferenceId = referenceId;
        }

        // Factory methods for creating different types of stock transactions
        // IN transaction adds stock, OUT transaction removes stock, ADJUST modifies stock by delta
        // Quantity is always positive for IN and OUT methods
        // For ADJUST, delta can be positive or negative
        // Example: StockTransaction.CreateIn(1, 1, null, 10, 5.00m, "PurchaseOrder", 1001);
        // Example: StockTransaction.CreateOut(1, 1, null, 5, 5.00m, "SalesOrder", 2001);
        // Example: StockTransaction.CreateAdjust(1, 1, null, -3, "InventoryCount", 3001);
        // Note: UnitCost is not applicable for ADJUST transactions
        // Note: ReferenceType and ReferenceId are optional metadata for tracking the source of the transaction
        public static StockTransaction CreateIn(
            int productId,
            int warehouseId,
            int? locationId,
            int quantity,
            decimal? unitCost = null,
            string? referenceType = null,
            string? referenceId = null)
        {
            if (quantity <= 0)
                throw new ArgumentException("Quantity must be greater than zero for IN transactions.", nameof(quantity));

            return new StockTransaction(productId, warehouseId, locationId, StockTransactionType.In, +quantity, unitCost, referenceType, referenceId);
        }

        public static StockTransaction CreateOut(
            int productId,
            int warehouseId,
            int? locationId,
            int quantity,
            string? referenceType = null,
            string? referenceId = null)
        {
            if (quantity <= 0)
                throw new ArgumentException("Quantity must be greater than zero for OUT transactions.", nameof(quantity));

            return new StockTransaction(productId, warehouseId, locationId, StockTransactionType.Out, -quantity, unitCost:null, referenceType, referenceId);
        }

        public static StockTransaction CreateAdjust(
            int productId,
            int warehouseId,
            int? locationId,
            int deltaQuantity,
            string? referenceType = null,
            string? referenceId = null)
        {
            // deltaQuantity can be + or -
            // + means increase stock, - means decrease stock
            if (deltaQuantity == 0) throw new ArgumentException("Adjustment delta cannot be zero.", nameof(deltaQuantity));

            return new StockTransaction(productId, warehouseId, locationId, StockTransactionType.Adjust, deltaQuantity, null, referenceType, referenceId);
        }

        // Transfer is usually two transactions (OUT + IN) in one DB transaction
    }
}
