
using IMS.Application.Common.Results;
using MediatR;

namespace IMS.Application.Features.Products.Queries.GetProductById
{
    // This query retrieves detailed information about a specific product by its unique identifier (ID).
    public sealed record GetProductByIdQuery(int id) : IRequest<Result<ProductDetailsDto>>;
}
