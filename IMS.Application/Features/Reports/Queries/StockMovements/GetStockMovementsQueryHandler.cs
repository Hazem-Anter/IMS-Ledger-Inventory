using IMS.Application.Abstractions.Read;
using IMS.Application.Common.Paging;
using IMS.Application.Common.Results;
using MediatR;

namespace IMS.Application.Features.Reports.Queries.StockMovements
{
    // Handler for the GetStockMovementsQuery,
    // responsible for processing the query and returning a paged result of stock movements.
    public sealed class GetStockMovementsQueryHandler
        : IRequestHandler<GetStockMovementsQuery, Result<PagedResult<StockMovementDto>>>
    {
        private readonly IStockReadService _read;

        public GetStockMovementsQueryHandler(IStockReadService read)
        {
            _read = read;
        }

        // Handles the GetStockMovementsQuery by validating the input parameters,
        public async Task<Result<PagedResult<StockMovementDto>>> Handle(
            GetStockMovementsQuery q,
            CancellationToken ct)
        {
            // 1) Validate that the 'ToUtc' date is after the 'FromUtc' date.
            if (q.ToUtc <= q.FromUtc)
                return Result<PagedResult<StockMovementDto>>
                    .Fail("Invalid date range. 'ToUtc' must be after 'FromUtc'.");

            // 2) Validate that the date range does not exceed 31 days to prevent excessively large queries.
            if ((q.ToUtc - q.FromUtc).TotalDays > 31)
                return Result<PagedResult<StockMovementDto>>
                    .Fail("Date range too large. Please request 31 days or less.");

            // 3) Validate that the page size is within acceptable limits (e.g., 1 to 200).
            var pageSize = q.PageSize > 200 ? 200 : q.PageSize;

            // 4) Validate that the page number is positive. If not, default to page 1.
            var page = q.Page > 0 ? q.Page : 1;

            // 5) Call the stock read service to retrieve
            // the paged stock movements based on the provided filters and pagination parameters.
            var rows = await _read.GetStockMovementsPagedAsync(
                                        q.FromUtc,
                                        q.ToUtc,
                                        q.WarehouseId,
                                        q.ProductId,
                                        page,
                                        pageSize,
                                        ct);

            // 6) Return the result wrapped in a success result object.
            return Result<PagedResult<StockMovementDto>>.Ok(rows);
        }
    }
}
