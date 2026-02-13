
using IMS.Application.Common.Paging;
using IMS.Application.Features.Locations.Queries.GetLocationById;
using IMS.Application.Features.Locations.Queries.ListLocations;

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
    }
}
