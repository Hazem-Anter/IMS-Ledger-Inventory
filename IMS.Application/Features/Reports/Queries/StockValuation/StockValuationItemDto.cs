
namespace IMS.Application.Features.Reports.Queries.StockValuation
{
    // This record defines the structure of a stock valuation item that will be included in the stock valuation report.
    public sealed record StockValuationItemDto(
        int ProductId,
        string ProductName,
        string Sku,
        int WarehouseId,
        string WarehouseCode,
        int? LocationId,
        string? LocationCode,
        int QuantityOnHand,
        decimal? UnitCost,
        decimal TotalValue
    );
}
