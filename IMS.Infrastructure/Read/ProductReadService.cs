
using IMS.Application.Abstractions.Read;
using IMS.Application.Common.Paging;
using IMS.Application.Features.Lookups.Dtos;
using IMS.Application.Features.Products.Queries.GetProductById;
using IMS.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace IMS.Infrastructure.Read
{
    // This class implements the IProductReadService interface,
    // providing methods to read product data from the database.
    public sealed class ProductReadService : IProductReadService
    {
        private readonly AppDbContext _db;

        public ProductReadService(AppDbContext db)
        {
            _db = db;
        }

        // Retrieves product details based on the provided barcode.
        public async Task<ProductDetailsDto?> GetByBarcode(string barcode, CancellationToken ct = default)
        {
            barcode = barcode.Trim();

            return await _db.Products
                .AsNoTracking()
                .Where(p => p.Barcode == barcode)
                .Select(p => new ProductDetailsDto(
                    p.Id, p.Name, p.Sku, p.Barcode, p.MinStockLevel, p.IsActive))
                .FirstOrDefaultAsync(ct);
        }

        // Retrieves product details based on the provided product ID.
        public async Task<ProductDetailsDto?> GetByIdAsync(int productId, CancellationToken ct = default)
        {
            return await _db.Products
                .AsNoTracking()
                .Where(p => p.Id == productId)
                .Select(p => new ProductDetailsDto(
                    p.Id, p.Name, p.Sku, p.Barcode, p.MinStockLevel, p.IsActive))
                .FirstOrDefaultAsync(ct);
        }

        // Lists products based on search criteria, active status, and pagination parameters.
        public async Task<PagedResult<ProductDetailsDto>> ListAsync(
            string? search,
            bool? isActive,
            int page,
            int pageSize,
            CancellationToken ct = default)
        {
            // 1) Start with the base query for products, using AsNoTracking for read-only operations.
            var q = _db.Products.AsNoTracking().AsQueryable();

            // 2) Apply the active status filter if provided.
            if (isActive.HasValue)
                q = q.Where(p => p.IsActive == isActive.Value);

            // 3) Apply the search filter if a search term is provided.
            // The search checks if the product's name, SKU, or barcode contains the search term.
            if (!string.IsNullOrWhiteSpace(search))
            {
                var s = search;//.Trim();
                q = q.Where(p =>
                    p.Name.Contains(s) ||
                    p.Sku.Contains(s) ||
                    (p.Barcode != null && p.Barcode.Contains(s)));
            }

            // 4) Count the total number of products that match the filters before applying pagination.
            var total = await q.CountAsync(ct);

            // 5) Apply sorting by product name, then apply pagination using Skip and Take.
            var items = await q
                .OrderBy(p => p.Name)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(p => new ProductDetailsDto(
                    p.Id, p.Name, p.Sku, p.Barcode, p.MinStockLevel, p.IsActive))
                .ToListAsync(ct);

            // 6) Return the paged result containing the list of products, total count, current page, and page size.
            return new PagedResult<ProductDetailsDto>(items, total, page, pageSize);
        }

        // Provides a lookup method to search for products based on a search term,
        // active status, and a limit on the number of results.
        public async Task<IReadOnlyList<ProductLookupDto>> LookupAsync(
            string? search,
            bool activeOnly,
            int take,
            CancellationToken ct = default)
        {
            // 1) Validate and adjust the 'take' parameter to ensure it falls within a reasonable range (1 to 50).
            if (take <= 0) take = 20;
            if(take > 50) take = 50;

            // 2) Start with the base query for products, using AsNoTracking for read-only operations.
            var q = _db.Products.AsNoTracking().AsQueryable();

            // 3) Apply the active status filter if 'activeOnly' is true,
            // ensuring only active products are included in the results.
            if (activeOnly)
                q = q.Where(p => p.IsActive);

            // 4) Apply the search filter if a search term is provided.
            if (!string.IsNullOrWhiteSpace(search))
            {
                var s = search.Trim();
                q = q.Where(p =>
                    p.Name.Contains(s) ||
                    p.Sku.Contains(s) ||
                    (p.Barcode != null && p.Barcode.Contains(s)));
            }

            // 5) Order the results by product name, limit the number of results to the specified 'take' value.
            return await q
                .OrderBy(p => p.Name)
                .Take(take)
                .Select(p => new ProductLookupDto(
                    p.Id,
                    p.Name,
                    p.Sku,
                    p.Barcode
                ))
                .ToListAsync(ct);

        }
    }
}
