
using IMS.Application.Abstractions.Caching;
using IMS.Application.Common.Results;
using MediatR;

namespace IMS.Application.Features.Inventory.Queries.StockOverview
{
    public sealed record GetStockOverviewQuery(
        int? WarehouseId = null,
        int? ProductId = null,
        bool LowStockOnly = false

    ) : IRequest<Result<List<StockOverviewItemDto>>>, IVersionedCacheableQuery
    {
        // 1) Cache prefix for grouping related cache entries
        // This is used for versioning the cache keys
        public string CachePrefix => "stock-overview";

        // 2) Cache key without versioning
        public string CacheKeyWithoutVersion =>
            $"w={WarehouseId?.ToString() ?? "all"}:p={ProductId?.ToString() ?? "all"}:low={LowStockOnly}";

        // 3) Full cache key including versioning
        public string CacheKey => $"{CachePrefix}:{CacheKeyWithoutVersion}";

        // 4) Sliding expiration time for the cache entry
        public TimeSpan? SlidingExpiration => TimeSpan.FromMinutes(2);
    }
}
