
using IMS.Application.Common.Paging;
using IMS.Application.Common.Results;
using MediatR;

namespace IMS.Application.Features.Reports.Queries.StockMovements
{
    // Query to get stock movements within a specified date range,
    // optionally filtered by product and warehouse, with pagination support.
    // Returns a paged result of StockMovementDto objects. 
    // Example usage: GetStockMovementsQuery(fromUtc, toUtc, productId, warehouseId, page, pageSize)
    // to retrieve stock movements. 
    // The query implements IRequest from MediatR to facilitate CQRS pattern.
    public sealed record GetStockMovementsQuery(
        DateTime FromUtc,
        DateTime ToUtc,
        int? WarehouseId,
        int? ProductId,

        // Pagination parameters
        int Page = 1,
        int PageSize = 50
        
        
     ) : IRequest<Result<PagedResult<StockMovementDto>>>;
}
