
using IMS.Application.Abstractions.Persistence;
using IMS.Application.Common.Results;
using IMS.Domain.Entities;
using MediatR;

namespace IMS.Application.Features.Products.Commands.DeactivateProduct
{
    // This handler processes the DeactivateProductCommand,
    // which deactivates a product by its unique identifier (ID).
    public sealed class DeactivateProductCommandHandler 
        : IRequestHandler<DeactivateProductCommand, Result<int>>
    {
        private readonly IRepository<Product> _products;
        private readonly IUnitOfWork _uow;

        public DeactivateProductCommandHandler(IRepository<Product> products, IUnitOfWork uow)
        {
            _products = products;
            _uow = uow;
        }

        public async Task<Result<int>> Handle(DeactivateProductCommand cmd, CancellationToken ct)
        {
            // 1) Retrieve the product by its ID from the repository.
            // If the product does not exist, return a failure result indicating that the product was not found.
            var product = await _products.GetByIdAsync(cmd.Id, ct);
            if (product is null) return Result<int>.Fail("Product not found.");
            
            // 2) Deactivate the product by calling the Activate method on the product entity.
            product.Deactivate();

            // 3) Update the product in the repository and save the changes using the unit of work.
            await _uow.SaveChangesAsync(ct);
            return Result<int>.Ok(product.Id);
        }
    }
}
