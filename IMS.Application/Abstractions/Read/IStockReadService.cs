
using IMS.Application.Features.Inventory.Queries.StockOverview;

namespace IMS.Application.Abstractions.Read
{
    public interface IStockReadService
    {
        Task<List<StockOverviewItemDto>> GetStockOverviewAsync(
            int? warehouseId = null,
            int? productId = null,
            bool lowStockOnly = false,

            CancellationToken ct = default);
    }
}
