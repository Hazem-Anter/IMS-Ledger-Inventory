
using IMS.Application.Common.Paging;
using IMS.Application.Features.Inventory.Queries.StockOverview;
using IMS.Application.Features.Reports.Queries.StockMovements;

namespace IMS.Application.Abstractions.Read
{
    public interface IStockReadService
    {
        // Gets an overview of stock Balance,
        // optionally filtered by warehouse, product, and low stock status.
        Task<List<StockOverviewItemDto>> GetStockOverviewAsync(
            int? warehouseId = null,
            int? productId = null,
            bool lowStockOnly = false,

            CancellationToken ct = default);

        // Retrieves stock movements within a specified date range,
        // optionally filtered by product and warehouse.
        Task<List<StockMovementDto>> GetStockMovementsAsync(
            DateTime fromUtc,
            DateTime toUtc,
            int? warehouseId = null,
            int? productId = null,
            CancellationToken ct = default
            );

        // Retrieves paged stock movements within a specified date range,
        // optionally filtered by product and warehouse.
        Task<PagedResult<StockMovementDto>> GetStockMovementsPagedAsync(
            DateTime fromUtc,
            DateTime toUtc,
            int? warehouseId,
            int? productId,
            int page,
            int pageSize,
            CancellationToken ct = default
            );

        // Retrieves a paged timeline of stock movements for a specific product within a date range,
        Task<PagedResult<StockMovementDto>> GetProductTimelinePagedAsync(
            int productId,
            DateTime fromUtc,
            DateTime toUtc,
            int? warehouseId,
            int page,
            int pageSize,
            CancellationToken ct = default);
    }
}
