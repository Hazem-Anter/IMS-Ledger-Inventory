
using IMS.Application.Abstractions.Read;
using IMS.Application.Common.Results;
using MediatR;

namespace IMS.Application.Features.Products.Queries.GetProductById
{
    // This handler processes the GetProductByIdQuery,
    // which retrieves detailed information about a specific product by its unique identifier (ID).
    public sealed class GetProductByIdQueryHandler
        : IRequestHandler<GetProductByIdQuery, Result<ProductDetailsDto>>
    {
        
        private readonly IProductReadService _read;
        public GetProductByIdQueryHandler(IProductReadService read)
        {
            _read = read;
        }

        public async Task<Result<ProductDetailsDto>> Handle(
            GetProductByIdQuery q, CancellationToken ct)
        {
            // 1) Use the injected IProductReadService to fetch the product details by ID.
            var product = await _read.GetByIdAsync(q.id, ct);

            // 2) Check if the product was found. If not, return a failure result with an appropriate error message.
            return product is null
                ? Result<ProductDetailsDto>.Fail("Product not found.")
                : Result<ProductDetailsDto>.Ok(product);
        }
    }
}
