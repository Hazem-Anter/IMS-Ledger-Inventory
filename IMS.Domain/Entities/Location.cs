
using IMS.Domain.Common;

namespace IMS.Domain.Entities
{
    public class Location : AuditableEntity
    {
        public int WarehouseId { get; private set; }
        public string Code { get; private set; } = default!;

        // Navigation property
        public Warehouse? Warehouse { get; private set; }

        private Location() { }
        public Location(int warehouseId, string code)
        {
            if (warehouseId <= 0)
                throw new ArgumentException("WarehouseId must be valid.", nameof(warehouseId));

            WarehouseId = warehouseId;
            SetCode(code);
        }

        public void SetCode(string code)
        {
            if (string.IsNullOrWhiteSpace(code))
                throw new ArgumentException("Location code is required.", nameof(code));

            Code = code.Trim().ToUpperInvariant();
        }
    }
}
