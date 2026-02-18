
using IMS.Application.Common.Paging;
using IMS.Application.Common.Results;
using MediatR;

namespace IMS.Application.Features.Warehouses.Queries.ListWarehouses
{
    // Query for listing warehouses with optional search and filtering parameters,
    // as well as pagination support.
    public sealed record ListWarehousesQuery(
        string? Search,
        bool? IsActive,
        int Page = 1,
        int PageSize = 10
    ) : IRequest<Result<PagedResult<WarehouseListItemDto>>>;
}
