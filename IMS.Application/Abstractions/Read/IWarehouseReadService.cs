
using IMS.Application.Common.Paging;
using IMS.Application.Features.Warehouses.Queries.GetWarehouseById;
using IMS.Application.Features.Warehouses.Queries.ListWarehouses;

namespace IMS.Application.Abstractions.Read
{
    // Read-only service interface for warehouse-related queries.
    // This is part of the CQRS pattern where we separate read and write operations.
    public interface IWarehouseReadService
    {
        Task<WarehouseDetailsDto?> GetByIdAsync(int id, CancellationToken ct = default);

        Task<PagedResult<WarehouseListItemDto>> ListAsync(
            string? search,
            bool ? isActive,
            int page,
            int pageSize,
            CancellationToken ct = default); 
    }
}
