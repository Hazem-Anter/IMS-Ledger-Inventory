
using IMS.Application.Common.Results;
using MediatR;

namespace IMS.Application.Features.Reports.Queries.StockMovements
{
    // Query to retrieve stock movements based on specified criteria
    // This query allows filtering stock movements by date range, product, and warehouse
    public sealed record GetStockMovementsQuery(
        DateTime FromUtc,
        DateTime ToUtc,
        int? ProductId,
        int? WarehouseId
        ) : IRequest<Result<List<StockMovementDto>>>;
}
