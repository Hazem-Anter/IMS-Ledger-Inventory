
using IMS.Application.Abstractions.Read;
using IMS.Application.Common.Paging;
using IMS.Application.Common.Results;
using MediatR;

namespace IMS.Application.Features.Locations.Queries.ListLocations
{
    // Handler for processing the ListLocationsQuery.
    // It uses the ILocationReadService to fetch a paginated list of locations for a specific warehouse,
    public sealed class ListLocationsQueryHandler
    : IRequestHandler<ListLocationsQuery, Result<PagedResult<LocationListItemDto>>>
    {
        private readonly ILocationReadService _read;

        public ListLocationsQueryHandler(ILocationReadService read)
        {
            _read = read;
        }

        public async Task<Result<PagedResult<LocationListItemDto>>> Handle(
            ListLocationsQuery q,
            CancellationToken ct)
        {
            // 1) Call the read service to get a paginated list of locations based on the query parameters.
            var items = await _read.ListByWarehouseAsync(
                                        q.WarehouseId,
                                        q.Search,
                                        q.IsActive,
                                        q.Page,
                                        q.PageSize,
                                        ct);

            // 2) Return the result wrapped in a Result object indicating success.
            return Result<PagedResult<LocationListItemDto>>.Ok(items);
        }
    }
}
