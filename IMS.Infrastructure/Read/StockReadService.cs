
using IMS.Application.Abstractions.Read;
using IMS.Application.Features.Inventory.Queries.StockOverview;
using IMS.Application.Features.Reports.Queries.StockMovements;
using IMS.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System.Linq;

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


        // Retrieves a list of stock overview items based on the provided filters
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

        // Retrieves a list of stock movements based on the provided criteria
        public async Task<List<StockMovementDto>> GetStockMovementsAsync(
            DateTime fromUtc,
            DateTime toUtc,
            int? productId = null,
            int? warehouseId = null,
            CancellationToken ct = default)
        {
            // 1) Validate the date range 
            // if the 'to' date is earlier than or equal to the 'from' date, return an empty list
            if (toUtc <= fromUtc)
                return new List<StockMovementDto>();

            // 2) Build the query to get stock transactions within the specified date range
            var query = _db.StockTransactions
                .AsNoTracking()
                .Where(t => t.CreatedAt >= fromUtc && t.CreatedAt <= toUtc);

            // 3) Apply filters based on the provided parameters
            if (productId is not null)
                query = query.Where(t => t.ProductId == productId);

            if (warehouseId is not null)
                query = query.Where(t => t.WarehouseId == warehouseId);

            // 4) Project the results into StockMovementDto and return the list
            return await query
                .OrderByDescending(t => t.CreatedAt)
                .Select(t => new StockMovementDto(
                    t.Id,
                    t.ProductId,
                    t.Product!.Name,
                    t.Product.Sku,
                    t.WarehouseId,
                    t.Warehouse!.Code,
                    t.LocationId == 0 ? null : t.LocationId,
                    t.LocationId == 0 ? null : t.Location!.Code,
                    t.Type.ToString(),
                    t.QuantityDelta,
                    t.UnitCost,
                    t.CreatedAt,
                    t.ReferenceType,
                    t.ReferenceId
                ))
                .ToListAsync(ct);
        }
    }
}
