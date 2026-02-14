
using IMS.Application.Abstractions.Read;
using IMS.Application.Features.Dashboard.Dtos;
using IMS.Application.Features.Reports.Queries.StockValuation;
using IMS.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace IMS.Infrastructure.Read
{
    public sealed class DashboardReadService : IDashboardReadService
    {
        private readonly AppDbContext _db;
        private readonly IStockReadService _stockRead;

        public DashboardReadService(
            AppDbContext db,
            IStockReadService stockRead)
        {
            _db = db;
            _stockRead = stockRead;
        }

        // This method aggregates various counts and values needed for the dashboard summary.
        public async Task<DashboardSummaryDto> GetSummaryAsync(CancellationToken ct = default)
        {
            // Products
            var totalProducts = await _db.Products.CountAsync(ct);

            var activeProducts = await _db.Products
                .Where(p => p.IsActive)
                .CountAsync(ct);

            // Warehouses
            var totalWarehouses = await _db.Warehouses.CountAsync(ct);

            var activeWarehouses = await _db.Warehouses
                .Where(w => w.IsActive)
                .CountAsync(ct);

            // Low stock count (reuse my read service)
            var lowStock = await _stockRead.GetLowStockReportAsync(null, null, ct);
            var lowStockItems = lowStock.Count;

            // Dead stock count
            // For simplicity, I'm using a fixed threshold of 30 days for dead stock.
            // In a real implementation, this could be configurable.
            var deadStock = await _stockRead.GetDeadStockReportAsync(30, null, ct);
            var deadStockItems = deadStock.Count;

            // Total stock value using your valuation logic
            var valuation = await _stockRead.GetStockValuationReportAsync(
                StockValuationMode.Fifo,
                null,
                null,
                ct);

            var totalStockValue = valuation.Sum(x => x.TotalValue);

            return new DashboardSummaryDto(
                totalProducts,
                activeProducts,
                totalWarehouses,
                activeWarehouses,
                lowStockItems,
                deadStockItems,
                totalStockValue
            );
        }
    }
}
