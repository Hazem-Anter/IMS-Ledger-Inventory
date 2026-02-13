
using IMS.Application.Common.Paging;
using IMS.Application.Common.Results;
using MediatR;

namespace IMS.Application.Features.Locations.Queries.ListLocations
{
    // Query for listing locations within a specific warehouse, with optional search and filtering parameters,
    public sealed record ListLocationsQuery(
        int WarehouseId,
        string? Search,
        bool? IsActive,
        int Page = 1,
        int PageSize = 20
    ) : IRequest<Result<PagedResult<LocationListItemDto>>>;
}
