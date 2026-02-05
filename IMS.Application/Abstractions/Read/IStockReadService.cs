
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

        Task<List<StockMovementDto>> GetStockMovementsAsync(
            DateTime fromUtc,
            DateTime toUtc,
            int? productId = null,
            int? warehouseId = null,
            CancellationToken ct = default
            );
    }
}
