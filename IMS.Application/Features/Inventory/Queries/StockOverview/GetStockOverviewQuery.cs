
using IMS.Application.Abstractions.Caching;
using IMS.Application.Common.Result;
using MediatR;

namespace IMS.Application.Features.Inventory.Queries.StockOverview
{
    public sealed record GetStockOverviewQuery(
        int? WarehouseId = null,
        int? ProductId = null,
        bool LowStockOnly = false

    ) : IRequest<Result<List<StockOverviewItemDto>>>, ICacheableQuery
    {
        public string CacheKey =>
            $"stock-overview:w={WarehouseId?.ToString() ?? "all"}:p={ProductId?.ToString() ?? "all"}:low={LowStockOnly}";
        public TimeSpan? SlidingExpiration => TimeSpan.FromMinutes(2);
    }
}
