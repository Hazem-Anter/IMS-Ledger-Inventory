
namespace IMS.Application.Features.Dashboard.Dtos
{
    // DTO for dashboard summary statistics
    // This record encapsulates key metrics for the inventory management system's dashboard,
    // providing a snapshot of the current state of products and warehouses.
    public sealed record DashboardSummaryDto(
        int TotalProducts,
        int ActiveProducts,
        int TotalWarehouses,
        int ActiveWarehouses,
        int LowStockItems,
        int DeadStockItems,
        decimal TotalStockValue
    );
}
