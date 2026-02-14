
using IMS.Application.Common.Paging;
using IMS.Application.Features.Locations.Queries.GetLocationById;
using IMS.Application.Features.Locations.Queries.ListLocations;
using IMS.Application.Features.Lookups.Dtos;

namespace IMS.Application.Abstractions.Read
{
    public interface ILocationReadService
    {
        Task<LocationDetailsDto?> GetByIdAsync(int warehouseId, int locationId, CancellationToken ct = default);

        Task<PagedResult<LocationListItemDto>> ListByWarehouseAsync(
            int warehouseId,
            string? search,
            bool? isActive,
            int page, 
            int pageSize,
            CancellationToken ct = default);

        // Get a list of locations for a warehouse, optionally filtered by search term and active status,
        // limited to a specified number of results.
        Task<IReadOnlyList<LocationLookupDto>> LookupByWarehouseAsync(
            int warehouseId,
            string? search,
            bool activeOnly,
            int take,
            CancellationToken ct = default);
    }
}
