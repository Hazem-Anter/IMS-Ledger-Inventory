
using IMS.Application.Common.Paging;
using IMS.Application.Common.Results;
using IMS.Application.Features.Reports.Queries.StockMovements;
using MediatR;

namespace IMS.Application.Features.Products.Queries.ProductTimeline
{
    // This query retrieves a paginated list of stock movements for a specific product,
    // optionally filtered by date range and warehouse.
    public sealed record GetProductTimelineQuery(
        int ProductId,
        DateTime? FromUtc = null,
        DateTime? ToUtc = null,
        int? WarehouseId = null,
        int Page = 1,
        int PageSize = 20
        ) : IRequest<Result<PagedResult<StockMovementDto>>>;
}
