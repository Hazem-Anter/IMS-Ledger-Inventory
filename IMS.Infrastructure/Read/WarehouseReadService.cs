
using IMS.Application.Abstractions.Read;
using IMS.Application.Common.Paging;
using IMS.Application.Features.Warehouses.Queries.GetWarehouseById;
using IMS.Application.Features.Warehouses.Queries.ListWarehouses;
using IMS.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace IMS.Infrastructure.Read
{
    // Implementation of the IWarehouseReadService interface,
    // responsible for handling read operations related to warehouses.
    public sealed class WarehouseReadService : IWarehouseReadService
    {
        private readonly AppDbContext _db;

        public WarehouseReadService(AppDbContext db)
        {
            _db = db;
        }

        // Retrieves the details of a warehouse by its ID.
        public async Task<WarehouseDetailsDto?> GetByIdAsync(int id, CancellationToken ct = default)
        {
            return await _db.Warehouses
                .AsNoTracking()
                .Where(w => w.Id == id)
                .Select(w => new WarehouseDetailsDto(w.Id, w.Name, w.Code, w.IsActive))
                .FirstOrDefaultAsync(ct);
        }

        // Lists warehouses based on search criteria, active status, and pagination parameters.
        public async Task<PagedResult<WarehouseListItemDto>> ListAsync(
            string? search,
            bool? isActive,
            int page,
            int pageSize,
            CancellationToken ct = default)
        {
            // 1) Start with the base query for warehouses, using AsNoTracking for read-only operations.
            var q = _db.Warehouses.AsNoTracking().AsQueryable();

            // 2) Apply filters based on the provided parameters.
            if (isActive.HasValue)
                q = q.Where(w => w.IsActive == isActive.Value);

            // 3) If a search term is provided, filter warehouses by name or code containing the search term.
            if (!string.IsNullOrWhiteSpace(search))
            {
                var s = search.Trim();
                q = q.Where(w => w.Name.Contains(s) || w.Code.Contains(s));
            }

            // 4) Get the total count of items matching the filters before applying pagination.
            var totalCount = await q.CountAsync(ct);

            // 5) Apply pagination and project the results to WarehouseListItemDto.
            var items = await q.
                OrderBy(w => w.Name)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(w => new WarehouseListItemDto(w.Id, w.Name, w.Code, w.IsActive))
                .ToListAsync(ct);

            // 6) Return the paged result containing the list of items, total count, current page, and page size.
            return new PagedResult<WarehouseListItemDto>(items, totalCount, page, pageSize);
        }
    }
}
