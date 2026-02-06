
namespace IMS.Application.Features.Reports.Queries.DeadStock
{
    // DTO representing a dead stock item in the report
    public sealed record DeadStockItemDto(
        int ProductId,
        string ProductName,
        string Sku,
        int WarehouseId,
        string WarehouseCode,
        int QuantityOnHand,
        DateTime? LastMovementAtUtc,
        int DaysSinceLastMovement
        );
}
