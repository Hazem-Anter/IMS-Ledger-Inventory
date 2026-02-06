
using IMS.Application.Abstractions.Read;
using IMS.Application.Common.Paging;
using IMS.Application.Common.Results;
using IMS.Application.Features.Reports.Queries.StockMovements;
using MediatR;

namespace IMS.Application.Features.Products.Queries.ProductTimeline
{
    // Handler for the GetProductTimelineQuery,
    // responsible for processing the query and returning a paged result of stock movements for a specific product.
    public sealed class GetProductTimelineQueryHandler 
        : IRequestHandler<GetProductTimelineQuery, Result<PagedResult<StockMovementDto>>>
    {
        private readonly IStockReadService _read;
        public GetProductTimelineQueryHandler(IStockReadService read)
        {
            _read = read;
        }

        // Handles the GetProductTimelineQuery and returns a Result containing a PagedResult of StockMovementDto.
        public async Task<Result<PagedResult<StockMovementDto>>> Handle(
            GetProductTimelineQuery q,
            CancellationToken ct)
        {
            // 1) Validate the product ID.
            if (q.ProductId <= 0)
                return Result<PagedResult<StockMovementDto>>
                    .Fail("Invalid product ID.");

            // 2) Validate the date range and set default values if necessary.
            var toUtc = q.ToUtc ?? DateTime.UtcNow;
            var fromUtc = q.FromUtc ?? toUtc.AddDays(-30);

            // Ensure the date range is valid (fromUtc must be before toUtc).
            if (toUtc <= fromUtc)
                return Result<PagedResult<StockMovementDto>>
                    .Fail("Invalid date range.");

            // 3) Validate pagination parameters and set defaults if necessary.
            var page = q.Page > 0 ? q.Page : 1;
            var pageSize = q.PageSize > 200 ? 200 : q.PageSize;

            // 4) Retrieve the paged stock movement data for the specified product and date range.
            var result = await _read.GetProductTimelinePagedAsync(
                q.ProductId,
                fromUtc,
                toUtc,
                q.WarehouseId,
                page,
                pageSize,
                ct);

            // 5) Return the result wrapped in a successful Result object.
            return Result<PagedResult<StockMovementDto>>.Ok(result);
        }
    }
}
