
using IMS.Domain.Common;

namespace IMS.Domain.Entities
{
    public class Product : AuditableEntity
    {
        public string Name { get; private set; } = default!;
        public string Sku { get; private set; } = default!;
        public string? Barcode { get; private set; }

        // Business Rule Support (alert)
        public int MinStockLevel { get; private set; }

        public bool IsActive { get; private set; } = true;

        // EF needs a parameterless constructor
        private Product() { }
        public Product(string name, string sku, string? barcode = null, int minStockLevel = 0)
        {
            SetName(name);
            SetSku(sku);
            SetBarcode(barcode);
            SetMinStockLevel(minStockLevel);
        }

        public void SetName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Product name is required.", nameof(name));
            
            Name = name.Trim();
        }
        public void SetSku(string sku)
        {
            if (string.IsNullOrWhiteSpace(sku))
                throw new ArgumentException("Sku is required.", nameof(sku));

            Sku = sku.Trim().ToUpperInvariant();
        }

        public void SetBarcode(string? barcode)
        {
            barcode = string.IsNullOrWhiteSpace(barcode) ? null : barcode.Trim();
            Barcode = barcode;
        }

        public void SetMinStockLevel(int minStockLevel)
        {
            if (minStockLevel < 0)
                throw new ArgumentOutOfRangeException(nameof(minStockLevel), "MinStockLevel cannot be negative.");

            MinStockLevel = minStockLevel;
        }

        public void Deactivate() => IsActive = false;
        public void Activate() => IsActive = true;
    }
}
