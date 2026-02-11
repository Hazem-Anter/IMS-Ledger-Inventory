
using IMS.Application.Common.Results;
using IMS.Application.Features.Products.Queries.GetProductById;
using MediatR;

namespace IMS.Application.Features.Products.Queries.GetProductByBarcode
{
    // This query retrieves detailed information about a specific product by its unique barcode.
    public sealed record GetProductByBarcodeQuery(string barcode) : IRequest<Result<ProductDetailsDto>>;
}
