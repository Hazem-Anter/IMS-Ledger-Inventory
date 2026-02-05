
namespace IMS.Application.Features.Reports.Queries.StockMovements
{
    // Data Transfer Object (DTO) representing a stock movement record
    public sealed record StockMovementDto(
        int TransactionId,
        int ProductId,
        string ProductName,
        string Sku,
        int WarehouseId,
        string WarehouseCode,
        int? LocationId,
        string? LocationCode,
        string Type,               // "In" / "Out" / "Adjust"
        int QuantityDelta,         // + for IN, - for OUT
        decimal? UnitCost,
        DateTime CreateAt,
        string? ReferenceType,
        string? ReferenceId

        );
}
