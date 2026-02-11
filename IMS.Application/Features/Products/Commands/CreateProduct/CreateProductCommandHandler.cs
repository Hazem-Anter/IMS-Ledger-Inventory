
using IMS.Application.Abstractions.Persistence;
using IMS.Application.Common.Results;
using IMS.Domain.Entities;
using MediatR;

namespace IMS.Application.Features.Products.Commands.CreateProduct
{
    // Handler for processing CreateProductCommand to add a new product to the inventory system
    public sealed class CreateProductCommandHandler
        : IRequestHandler<CreateProductCommand, Result<int>>
    {
        private readonly IRepository<Product> _products;
        private readonly IUnitOfWork _uow;

        public CreateProductCommandHandler(IRepository<Product> products, IUnitOfWork uow)
        {
            _products = products;
            _uow = uow;
        }

        public async Task<Result<int>> Handle(
            CreateProductCommand cmd,
            CancellationToken ct)
        {
            // 1) Validate and normalize input 
            //   - SKU is normalized to uppercase and trimmed
            //  - Barcode is optional, but if provided, it is trimmed
            var sku = cmd.Sku.Trim().ToUpperInvariant();
            var barcode = string.IsNullOrWhiteSpace(cmd.Barcode) ? null : cmd.Barcode.Trim();

            // 2) Check for uniqueness of SKU and Barcode
            if (await _products.AnyAsync(p => p.Sku == cmd.Sku, ct))
                return Result<int>.Fail("SKU already exists.");

            if (barcode is not null && await _products.AnyAsync(p => p.Barcode == barcode, ct))
                return Result<int>.Fail("Barcode already exists.");

            // 3) Create new Product entity and add to repository, if validation passes
            var product = new Product(cmd.Name, sku, barcode, cmd.MinStockLevel);

            await _products.AddAsync(product, ct);

            // 4) Save changes to generate Identity ID for the new product
            // IMPORTANT: Identity ID is generated only after SaveChanges
            await _uow.SaveChangesAsync(ct);

            return Result<int>.Ok(product.Id);

        }
    }
}
