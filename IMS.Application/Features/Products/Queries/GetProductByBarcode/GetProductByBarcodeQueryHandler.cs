
using IMS.Application.Abstractions.Read;
using IMS.Application.Common.Results;
using IMS.Application.Features.Products.Queries.GetProductById;
using MediatR;

namespace IMS.Application.Features.Products.Queries.GetProductByBarcode
{
    // This handler processes the GetProductByBarcodeQuery,
    // which retrieves detailed information about a specific product by its unique barcode.
    public sealed class GetProductByBarcodeQueryHandler
        : IRequestHandler<GetProductByBarcodeQuery, Result<ProductDetailsDto>>
    {

        private readonly IProductReadService _read;

        public GetProductByBarcodeQueryHandler(IProductReadService read)
        {
            _read = read;
        }

        public async Task<Result<ProductDetailsDto>> Handle(
            GetProductByBarcodeQuery q, CancellationToken ct)
        {
            // 1) Trim the input barcode to remove any leading or trailing whitespace.
            var barcode = q.barcode.Trim();

            // 2) Use the IProductReadService to attempt to retrieve the product details associated with the provided barcode.
            var product = await _read.GetByBarcode(barcode);

            // 3) If the product is not found (i.e., the result is null),
            // return a failure result with an appropriate error message.
            return product is null
                ? Result<ProductDetailsDto>.Fail("Product not found.")
                : Result<ProductDetailsDto>.Ok(product);
        }
    }
}
