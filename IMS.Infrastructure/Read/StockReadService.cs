
using IMS.Application.Abstractions.Read;
using IMS.Application.Features.Inventory.Queries.StockOverview;
using IMS.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace IMS.Infrastructure.Read
{
    // Implementation of the stock read service 
    public class StockReadService : IStockReadService
    {
        private readonly AppDbContext _db;
        public StockReadService(AppDbContext db)
        {
            _db = db;
        }
        public async Task<List<StockOverviewItemDto>> GetStockOverviewAsync(
            int? warehouseId = null,
            int? productId = null,
            bool lowStockOnly = false,
            CancellationToken ct = default)
        {
            // 1) Build the query to get stock balances with non-zero quantity 
            var query = _db.StockBalances
            .AsNoTracking()
            .Where(b => b.QuantityOnHand != 0);

            // 2) Apply filters based on the provided parameters
            if (warehouseId is not null)
                query = query.Where(b => b.WarehouseId == warehouseId);

            if (productId is not null)
                query = query.Where(b => b.ProductId == productId);

            if (lowStockOnly)
                query = query.Where(b => b.QuantityOnHand <= b.Product!.MinStockLevel);

            // 3) Project the results into StockOverviewItemDto and return the list 
            return await query
                .OrderBy(b => b.ProductId)
                .ThenBy(b => b.WarehouseId)
                .ThenBy(b => b.LocationId)
                .Select(b => new StockOverviewItemDto(
                    b.ProductId,
                    b.Product!.Name,
                    b.Product.Sku,
                    b.WarehouseId,
                    b.Warehouse!.Code,
                    b.LocationId,
                    b.Location != null ? b.Location.Code : null,
                    b.QuantityOnHand
                ))
                .ToListAsync(ct);

            // we did not use include statements to load related entities (Product, Warehouse, Location)
            // beacause we are only interested in specific fields for the DTO projection.
            // and "Select" statement will handle the necessary joins efficiently.
        }
    }
}
