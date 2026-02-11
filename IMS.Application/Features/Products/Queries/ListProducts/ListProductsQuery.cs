
using IMS.Application.Common.Paging;
using IMS.Application.Common.Results;
using IMS.Application.Features.Products.Queries.GetProductById;
using MediatR;

namespace IMS.Application.Features.Products.Queries.ListProducts
{
    // This query retrieves a paginated list of products based on optional search criteria,
    // such as a search term and an active status filter.
    public sealed record ListProductsQuery(
        string? Search,
        bool? IsActive,
        int Page,
        int PageSize 
        ) : IRequest<Result<PagedResult<ProductDetailsDto>>>;
}
