
using IMS.Application.Abstractions.Read;
using IMS.Application.Common.Paging;
using IMS.Application.Common.Results;
using IMS.Application.Features.Products.Queries.GetProductById;
using MediatR;

namespace IMS.Application.Features.Products.Queries.ListProducts
{
    // This handler processes the ListProductsQuery,
    // which retrieves a paginated list of products based on optional search criteria,
    public sealed class ListProductsQueryHandler
        : IRequestHandler<ListProductsQuery, Result<PagedResult<ProductDetailsDto>>>
    {
        private readonly IProductReadService _read;

        public ListProductsQueryHandler(IProductReadService read)
        {
            _read = read;
        }

        public async Task<Result<PagedResult<ProductDetailsDto>>> Handle(
            ListProductsQuery q,
            CancellationToken ct)
        {
            // 1) Validate the query parameters (e.g., page number and page size).
            var data = await _read.ListAsync(q.Search, q.IsActive, q.Page, q.PageSize, ct);

            // 2) If validation fails, return a failure result with an appropriate error message.
            return data is null
                ? Result<PagedResult<ProductDetailsDto>>.Fail("Failed to retrieve products.")
                : Result<PagedResult<ProductDetailsDto>>.Ok(data);
        }

    }
}
