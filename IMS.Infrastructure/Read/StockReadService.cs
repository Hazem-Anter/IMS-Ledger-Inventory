
using IMS.Application.Abstractions.Read;
using IMS.Application.Common.Paging;
using IMS.Application.Features.Inventory.Queries.StockOverview;
using IMS.Application.Features.Reports.Queries.DeadStock;
using IMS.Application.Features.Reports.Queries.LowStock;
using IMS.Application.Features.Reports.Queries.StockMovements;
using IMS.Application.Features.Reports.Queries.StockValuation;
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
            int? warehouseId = null,
            int? productId = null,
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

        // Retrieves a paged list of stock movements based on the provided criteria
        // This method is similar to GetStockMovementsAsync,
        // but includes pagination parameters (page and pageSize),
        // and returns a PagedResult containing the items and total count.
        public async Task<PagedResult<StockMovementDto>> GetStockMovementsPagedAsync(
            DateTime fromUtc,
            DateTime toUtc,
            int? warehouseId,
            int? productId,
            int page,
            int pageSize,
            CancellationToken ct = default)
        {

            // 1) Validate the date range
            if (toUtc <= fromUtc)
                return new PagedResult<StockMovementDto>(Array.Empty<StockMovementDto>(), 0, page, pageSize);

            // 2) Ensure page and pageSize are within reasonable limits to prevent abuse
            if (page <= 0)
                page = 1;
            if(pageSize <= 0)
                pageSize = 50;
            if(pageSize > 200)
                pageSize = 200;

            // 3) Build the base query to get stock transactions within the specified date range
            var baseQuery = _db.StockTransactions
                .AsNoTracking()
                .Where(t => t.CreatedAt >= fromUtc && t.CreatedAt <= toUtc);

            // 4) Apply filters based on the provided parameters
            if (warehouseId is not null)
                baseQuery = baseQuery.Where(t => t.WarehouseId == warehouseId);

            if(productId is not null)
                baseQuery = baseQuery.Where(t => t.ProductId == productId);

            // 5) Get the total count of items matching the criteria (before pagination)
            var totalCount = await baseQuery.CountAsync(ct);

            // 6) Apply pagination to the query and project the results into StockMovementDto
            var items = await baseQuery
                .OrderByDescending(t => t.CreatedAt)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
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

            // 7) Return the paged result containing the items and total count
            return new PagedResult<StockMovementDto>(items, totalCount, page, pageSize);
        }

        public async Task<PagedResult<StockMovementDto>> GetProductTimelinePagedAsync(
            int productId,
            DateTime fromUtc,
            DateTime toUtc,
            int? warehouseId,
            int page,
            int pageSize,
            CancellationToken ct = default)
        {
            
            // 1) Ensure page and pageSize are within reasonable limits to prevent abuse
            if (page <= 0) page = 1;
            if (pageSize <= 0) pageSize = 50;
            if (pageSize > 200) pageSize = 200;

            // 2) Build the base query to get stock transactions for the specified product and date range
            var baseQuery = _db.StockTransactions
                .AsNoTracking()
                .Where(t =>
                    t.ProductId == productId &&
                    t.CreatedAt >= fromUtc &&
                    t.CreatedAt <= toUtc);

            // 3) Apply warehouse filter if provided
            if (warehouseId is not null)
                baseQuery = baseQuery.Where(t => t.WarehouseId == warehouseId);

            // 4) Get the total count of items matching the criteria (before pagination)
            var totalCount = await baseQuery.CountAsync(ct);

            // 6) Apply pagination to the query and project the results into StockMovementDto
            var items = await baseQuery
                .OrderByDescending(t => t.CreatedAt)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
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

            // 5) Return the paged result containing the items and total count
            return new PagedResult<StockMovementDto>(items, totalCount, page, pageSize);
        }


        // Retrieves a list of low stock items based on the provided filters
        public async Task<List<LowStockItemDto>> GetLowStockReportAsync(
            int? warehouseId,
            int? productId,
            CancellationToken ct = default)
        {
            // 1) Build the query to get stock balances
            // where quantity on hand is less than or equal to the product's minimum stock level
            var query = _db.StockBalances
                .AsNoTracking()
                .Where(b => b.QuantityOnHand <= b.Product!.MinStockLevel);

            // 2) Apply filters based on the provided parameters
            if (warehouseId is not null)
                query = query.Where(b => b.WarehouseId == warehouseId);

            if (productId is not null)
                query = query.Where(b => b.ProductId == productId);

            // 3) Project the results into LowStockItemDto and return the list
            return await query
                .OrderBy(b => b.ProductId)
                .ThenBy(b => b.WarehouseId)
                .ThenBy(b => b.LocationId)
                .Select(b => new LowStockItemDto(
                    b.ProductId,
                    b.Product!.Name,
                    b.Product.Sku,
                    b.WarehouseId,
                    b.Warehouse!.Code,
                    b.LocationId == 0 ? null : b.LocationId,
                    b.LocationId == 0 ? null : b.Location!.Code,
                    b.QuantityOnHand,
                    b.Product.MinStockLevel,
                    b.Product.MinStockLevel - b.QuantityOnHand
                ))
                .ToListAsync(ct);

        }


        // Retrieves a list of dead stock items based on the provided criteria
        public async Task<List<DeadStockItemDto>> GetDeadStockReportAsync(
            int days,
            int? warehouseId,
            CancellationToken ct = default)
        {
            // 1) Calculate the cutoff date by subtracting the specified number of days from the current date
            var cutoffDate = DateTime.UtcNow.AddDays(-days);

            // 2) Build the query to get stock balances with quantity on hand greater than zero
            var balances = _db.StockBalances
                .AsNoTracking()
                .Where(b => b.QuantityOnHand > 0);

            // 3) Apply warehouse filter if provided
            if (warehouseId is not null)
                balances = balances.Where(b => b.WarehouseId == warehouseId);

            // 4) For each stock balance, find the date of the last stock movement (transaction) for that product and warehouse
            // If there are no movements, the last movement date will be null.
            var query =
                from b in balances
                let lastMovement = _db.StockTransactions
                    .Where(t =>
                        t.ProductId == b.ProductId &&
                        t.WarehouseId == b.WarehouseId)
                    .Max(t => (DateTime?)t.CreatedAt)
                where lastMovement == null || lastMovement < cutoffDate
                orderby lastMovement
                select new DeadStockItemDto(
                    b.ProductId,
                    b.Product!.Name,
                    b.Product.Sku,
                    b.WarehouseId,
                    b.Warehouse!.Code,
                    b.QuantityOnHand,
                    lastMovement,
                    lastMovement == null
                        ? days
                        : (int)(DateTime.UtcNow - lastMovement.Value).TotalDays
                );

            return await query.ToListAsync(ct);
        }

        // ##################################################################################
        // ################################# Valuation ######################################
        // ##################################################################################

        // Helper record to represent an IN layer with quantity and unit cost
        // This is used to compute FIFO valuation by simulating the layers of stock received.
        // Each IN transaction creates a layer of stock with a certain quantity and unit cost.
        private sealed record InLayer(int Qty, decimal UnitCost);


        // Computes the FIFO value of the on-hand quantity based on the layers of stock received.
        // The method simulates the consumption of stock from the layers in FIFO order,
        // and calculates the value of the remaining stock.
        // The layers are processed from the most recent to the oldest, taking into account the quantity on hand.
        private static decimal ComputeFifoValue(int onHandQty, List<InLayer> layers)
        {
            // 1) If there is no stock on hand or no layers, the value is zero.
            if (onHandQty <= 0 || layers.Count == 0)
                return 0m;

            // 2) Calculate the total quantity received from the layers.

            // FIFO: remaining stock comes from earliest layers,
            // but easiest way to compute remaining value:
            // Take the last "onHandQty" units from the layered flow after consumption.
            // We'll simulate consumption from layers until remaining = onHandQty.

            var totalIn = layers.Sum(l => l.Qty);
            if (totalIn <= 0)
                return 0m;

            // 3) Determine how many units we need to keep based on the on-hand quantity.

            // If we received less than or equal current on-hand, then all received is still on hand
            // (This can happen if OUT tracking isn't included; but our stock balance should match.)
            // We'll still calculate based on onHandQty.
            var toKeep = Math.Min(onHandQty, totalIn);

            // 4) Compute the value of the remaining stock by taking from the layers in reverse order (newest to oldest).

            // We need to keep the last 'toKeep' units AFTER consumption.
            // FIFO keeps oldest sold first -> remaining comes from newer layers.
            // So compute value by taking from the end (reverse layers).
            decimal value = 0m;
            int remaining = toKeep;
            
            for (int i = layers.Count - 1; i >= 0 && remaining > 0; i--)
            {
                var take = Math.Min(remaining, layers[i].Qty);
                value += take * layers[i].UnitCost;
                remaining -= take;
            }

            return value;
        }

        // Retrieves a list of stock valuation items based on the specified valuation mode and filters.
        public async Task<List<StockValuationItemDto>> GetStockValuationReportAsync(
            StockValuationMode mode,
            int? warehouseId,
            int? productId,
            CancellationToken ct = default)
        {
            // 1) Build the query to get stock balances with quantity on hand greater than zero
            var balances = _db.StockBalances
                .AsNoTracking()
                .Where(b => b.QuantityOnHand > 0);

            // 2) Apply filters based on the provided parameters
            if (warehouseId is not null)
                balances = balances.Where(b => b.WarehouseId == warehouseId);

            if (productId is not null)
                balances = balances.Where(b => b.ProductId == productId);

            // 3) Get the relevant data for the stock balances, including product and warehouse information.
            // We will need the product name, SKU, warehouse code, and quantity on hand for each balance to compute the valuation.
            var balanceRows = await balances
                .Select(b => new
                {
                    b.ProductId,
                    ProductName = b.Product!.Name,
                    Sku = b.Product.Sku,
                    b.WarehouseId,
                    WarehouseCode = b.Warehouse!.Code,
                    b.LocationId,
                    LocationCode = b.Location!.Code,
                    b.QuantityOnHand
                })
                .ToListAsync(ct);

            // results list to hold the computed stock valuation items  
            var results = new List<StockValuationItemDto>();


            // 4) For each stock balance, fetch the relevant IN transactions with unit cost to compute the valuation based on the specified mode (FIFO or Weighted Average).
            foreach (var b in balanceRows)
            {
                // Fetch IN layers with UnitCost
                var inTx = await _db.StockTransactions
                    .AsNoTracking()
                    .Where(t =>
                        t.ProductId == b.ProductId &&
                        t.WarehouseId == b.WarehouseId &&
                        t.Type == Domain.Enum.StockTransactionType.In &&  // Only consider IN transactions with unit cost for valuation
                        t.UnitCost != null)
                    .OrderBy(t => t.CreatedAt)
                    .Select(t => new InLayer(t.QuantityDelta, t.UnitCost!.Value)) // QuantityDelta should be + for IN
                    .ToListAsync(ct);

                // Note: We are only considering IN transactions with a unit cost for valuation purposes.
                // OUT transactions do not have a unit cost and are not needed for computing the valuation of the remaining stock.

                // Safety: ensure positive qty
                var layers = inTx
                    .Where(x => x.Qty > 0)
                    .ToList();

                
                decimal totalValue = 0m;
                decimal? unitCost = null;

                // 5) Compute the total value and unit cost based on the specified valuation mode.
                // For FIFO, we compute the value of the remaining stock by simulating the consumption of stock from the layers in FIFO order.
                // For Weighted Average, we calculate the average unit cost across all layers and then multiply by the quantity on hand to get the total value.
                if (mode == StockValuationMode.Fifo)
                {
                    totalValue = ComputeFifoValue(b.QuantityOnHand, layers);
                    unitCost = b.QuantityOnHand == 0 ? null : (totalValue / b.QuantityOnHand);
                }
                else // WeightedAverage
                {
                    var totalQty = layers.Sum(x => x.Qty);
                    var totalCost = layers.Sum(x => x.Qty * x.UnitCost);
                    unitCost = totalQty == 0 ? null : totalCost / totalQty;
                    totalValue = unitCost == null ? 0m : b.QuantityOnHand * unitCost.Value;
                }

                // 6) Create a StockValuationItemDto for each stock balance with the computed unit cost and total value, and add it to the results list.
                results.Add(new StockValuationItemDto(
                    b.ProductId,
                    b.ProductName,
                    b.Sku,
                    b.WarehouseId,
                    b.WarehouseCode,
                    b.LocationId,
                    b.LocationCode,
                    b.QuantityOnHand,
                    unitCost,
                    totalValue
                ));
            }


            // 7) Return the list of StockValuationItemDto, ordered by total value in descending order.
            return results
                .OrderByDescending(r => r.TotalValue)
                .ToList();
        }


        // ##################################################################################
        // ################################# Valuation ######################################
        // ##################################################################################

    }
}
