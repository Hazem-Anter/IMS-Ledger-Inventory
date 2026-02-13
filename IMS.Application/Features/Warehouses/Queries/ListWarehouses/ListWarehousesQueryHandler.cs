
using IMS.Application.Abstractions.Read;
using IMS.Application.Common.Paging;
using IMS.Application.Common.Results;
using MediatR;

namespace IMS.Application.Features.Warehouses.Queries.ListWarehouses
{
    // Handler for the ListWarehousesQuery,
    // responsible for processing the query and returning a paginated list of warehouses based on the provided search and filter criteria.
    public sealed class ListWarehousesQueryHandler 
        : IRequestHandler<ListWarehousesQuery, Result<PagedResult<WarehouseListItemDto>>>
    {
        private readonly IWarehouseReadService _read;

        public ListWarehousesQueryHandler(IWarehouseReadService read)
        {
            _read = read;
        }

        public async Task<Result<PagedResult<WarehouseListItemDto>>> Handle(
            ListWarehousesQuery q, CancellationToken ct)
        {
            // 1) Use the IWarehouseReadService to retrieve a paginated list of warehouses based on the search and filter criteria provided in the query.
            var result =await _read.ListAsync(q.Search, q.IsActive, q.Page, q.PageSize, ct);

            // 2) Check if the result is null, and return a failure result with an appropriate error message if it is.
            // Otherwise, return a successful result containing the paginated list of warehouses.
            return result is null 
                ? Result<PagedResult<WarehouseListItemDto>>.Fail("Failed to retrieve warehouses.")
                : Result<PagedResult<WarehouseListItemDto>>.Ok(result);

        }
    }
}
