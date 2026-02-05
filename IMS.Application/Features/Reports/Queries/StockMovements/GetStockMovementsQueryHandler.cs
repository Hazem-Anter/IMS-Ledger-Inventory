using IMS.Application.Abstractions.Read;
using IMS.Application.Common.Results;
using MediatR;

namespace IMS.Application.Features.Reports.Queries.StockMovements
{
    // Handler for retrieving stock movements within a specified date range and optional filters.
    // Validates input and interacts with the read service to fetch data.
    public sealed class GetStockMovementsQueryHandler
        : IRequestHandler<GetStockMovementsQuery, Result<List<StockMovementDto>>>
    {
        private readonly IStockReadService _read;

        public GetStockMovementsQueryHandler(IStockReadService read)
        {
            _read = read;
        }

        // Handles the GetStockMovementsQuery to retrieve stock movement data.
        public async Task<Result<List<StockMovementDto>>> Handle(
            GetStockMovementsQuery q,
            CancellationToken ct)
        {
            // 1) Validate date range inputs 
            // 'ToUtc' must be after 'FromUtc' 
            if (q.ToUtc <= q.FromUtc)
                return Result<List<StockMovementDto>>
                    .Fail("Invalid date range. 'ToUtc' must be after 'FromUtc'.");

            // 2) Ensure the date range does not exceed 31 days
            // This prevents overly large data requests.
            if ((q.ToUtc - q.FromUtc).TotalDays > 31)
                return Result<List<StockMovementDto>>
                    .Fail("Date range too large. Please request 31 days or less.");

            // 3) Fetch stock movements from the read service
            var rows = await _read.GetStockMovementsAsync(
                                        q.FromUtc,
                                        q.ToUtc,
                                        q.WarehouseId,
                                        q.ProductId,
                                        ct);

            // 4) Return the retrieved stock movements wrapped in a success result
            return Result<List<StockMovementDto>>.Ok(rows);
        }
    }
}
