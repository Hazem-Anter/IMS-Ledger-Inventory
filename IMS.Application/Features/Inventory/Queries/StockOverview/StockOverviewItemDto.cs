
namespace IMS.Application.Features.Inventory.Queries.StockOverview
{
    // DTO representing a stock overview item 
    public sealed record StockOverviewItemDto(
        int ProductId,
        string ProductName,
        string Sku,
        int WarehouseId,
        string WarehouseCode,
        int? LocationId,
        string LocationCode,
        int QuantityOnHand
    );
}
