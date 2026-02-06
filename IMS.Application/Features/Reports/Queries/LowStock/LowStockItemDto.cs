
namespace IMS.Application.Features.Reports.Queries.LowStock
{
    // DTO to represent a low stock item in the report
    public sealed record LowStockItemDto(
        int ProductId,
        string ProductName,
        string Sku,
        int WarehouseId,
        string WarehouseCode,
        int? LocationId,
        string? LocationCode,
        int QuantityOnHand,
        int MinStockLevel,
        int Shortage // how many units missing to reach min level
        );
}
