
using IMS.Application.Abstractions.Read;
using IMS.Application.Common.Paging;
using IMS.Application.Features.Locations.Queries.GetLocationById;
using IMS.Application.Features.Locations.Queries.ListLocations;
using IMS.Application.Features.Lookups.Dtos;
using IMS.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace IMS.Infrastructure.Read
{
    // Implementation of the ILocationReadService interface for reading location data from the database.
    public sealed class LocationReadService : ILocationReadService
    {
        private readonly AppDbContext _db;

        public LocationReadService(AppDbContext db)
        {
            _db = db;
        }

        // Retrieves the details of a specific location by its ID and associated warehouse ID.
        public async Task<LocationDetailsDto?> GetByIdAsync(
            int warehouseId,
            int locationId,
            CancellationToken ct = default)
        {
            return await _db.Locations
                .AsNoTracking()
                .Where(l => l.Id == locationId && l.WarehouseId == warehouseId)
                .Select(l => new LocationDetailsDto(l.Id, l.WarehouseId, l.Code, l.IsActive))
                .FirstOrDefaultAsync(ct);
        }

        // Retrieves a paginated list of locations for a specific warehouse, with optional search and filtering by active status.
        public async Task<PagedResult<LocationListItemDto>> ListByWarehouseAsync(
            int warehouseId,
            string? search,
            bool? isActive,
            int page,
            int pageSize,
            CancellationToken ct = default)
        {
            // 1) Build the base query for locations in the specified warehouse.
            var q = _db.Locations
                    .AsNoTracking()
                    .Where(l => l.WarehouseId == warehouseId)
                    .AsQueryable();

            // 2) Apply filtering by active status if specified.
            if (isActive.HasValue)
                q = q.Where(l => l.IsActive == isActive.Value);

            // 3) Apply search filtering on the location code if a search term is provided.
            if (!string.IsNullOrWhiteSpace(search))
            {
                var s = search.Trim();
                q = q.Where(l => l.Code.Contains(s));
            }

            // 4) Get the total count of items that match the filters before applying pagination.
            var totalCount = await q.CountAsync(ct);

            // 5) Apply pagination and project the results to a list of LocationListItemDto.
            var items = await q
                            .OrderBy(l => l.Code)
                            .Skip((page - 1) * pageSize)
                            .Take(pageSize)
                            .Select(l => new LocationListItemDto(l.Id, l.WarehouseId, l.Code, l.IsActive))
                            .ToListAsync(ct);

            // 6) Return the paginated result containing the list of items and pagination metadata.
            return new PagedResult<LocationListItemDto>(items, totalCount, page, pageSize);
        }

        // Retrieves a list of locations for lookup purposes, filtered by warehouse ID, optional search term,
        // and active status, with a limit on the number of results returned.
        public async Task<IReadOnlyList<LocationLookupDto>> LookupByWarehouseAsync(
            int warehouseId,
            string? search,
            bool activeOnly,
            int take,
            CancellationToken ct = default)
        {
            // 1) Validate and adjust the 'take' parameter to ensure it falls within a reasonable range.
            if (take <= 0)
                take = 20;
            if(take > 200)
                take = 200;

            // 2) Build the base query for locations in the specified warehouse.
            var q = _db.Locations
                    .AsNoTracking()
                    .Where(l => l.WarehouseId == warehouseId)
                    .AsQueryable();

            // 3) Apply filtering to include only active locations if 'activeOnly' is true.
            if (activeOnly)
                q = q.Where(l => l.IsActive);

            // 4) Apply search filtering on the location code if a search term is provided.
            if (!string.IsNullOrWhiteSpace(search))
            {
                var s = search.Trim();
                q = q.Where(l => l.Code.Contains(s));
            }

            // 5) Order the results by location code, limit the number of results to 'take',
            // and project to a list of LocationLookupDto.
            return await q
                    .OrderBy(l => l.Code)
                    .Take(take)
                    .Select(l => new LocationLookupDto(l.Id, l.Code ))
                    .ToListAsync(ct);

        }
    }
}
