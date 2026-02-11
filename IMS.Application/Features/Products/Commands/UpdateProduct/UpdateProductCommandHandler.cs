
using IMS.Application.Abstractions.Persistence;
using IMS.Application.Common.Results;
using IMS.Domain.Entities;
using MediatR;

namespace IMS.Application.Features.Products.Commands.UpdateProduct
{
    // This handler processes the UpdateProductCommand, 
    // which updates the details of an existing product in the inventory system.
    public sealed class UpdateProductCommandHandler
        : IRequestHandler<UpdateProductCommand, Result<int>>
    {
        private readonly IRepository<Product> _products;
        private readonly IUnitOfWork _uow;

        public UpdateProductCommandHandler(IRepository<Product> products, IUnitOfWork uow)
        {
            _products = products;
            _uow = uow;
        }

        public async Task<Result<int>> Handle(
            UpdateProductCommand cmd,
            CancellationToken ct)
        {
            // 1) Retrieve existing product, fail if not found
            var product = await _products.GetByIdAsync(cmd.Id, ct);
            if (product is null)
                return Result<int>.Fail("Product not found.");

            // 2) Normalize inputs
            var sku = cmd.Sku.Trim().ToUpperInvariant();
            var barcode = string.IsNullOrWhiteSpace(cmd.Barcode) ? null : cmd.Barcode.Trim();

            // 3) Check for SKU and Barcode uniqueness
            if (await _products.AnyAsync(p => p.Id != cmd.Id && p.Sku == sku, ct))
                return Result<int>.Fail("SKU already exists.");

            if (barcode is not null && await _products.AnyAsync(p => p.Id != cmd.Id && p.Barcode == barcode, ct))
                return Result<int>.Fail("Barcode already exists.");

            // 4) Update product properties if all validations pass
            product.SetName(cmd.Name);
            product.SetSku(sku);
            product.SetBarcode(barcode);
            product.SetMinStockLevel(cmd.MinStockLevel);

            // 5) Persist changes
            await _uow.SaveChangesAsync(ct); 

            return Result<int>.Ok(product.Id);
        }
    }
}
