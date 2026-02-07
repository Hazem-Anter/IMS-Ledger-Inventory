
using IMS.Application.Abstractions.Read;
using IMS.Application.Common.Results;
using MediatR;

namespace IMS.Application.Features.Reports.Queries.StockValuation
{
    // This class handles the GetStockValuationReportQuery by implementing the IRequestHandler interface from MediatR.
    public sealed class GetStockValuationReportQueryHandler
    : IRequestHandler<GetStockValuationReportQuery, Result<List<StockValuationItemDto>>>
    {
        private readonly IStockReadService _read;

        public GetStockValuationReportQueryHandler(IStockReadService read)
        {
            _read = read;
        }

        public async Task<Result<List<StockValuationItemDto>>> Handle(
            GetStockValuationReportQuery q,
            CancellationToken ct)
        {
            // 1) Call the GetStockValuationReportAsync method of the IStockReadService to retrieve the stock valuation report based on the query parameters.
            var rows = await _read.GetStockValuationReportAsync(
                                    q.Mode,
                                    q.WarehouseId,
                                    q.ProductId,
                                    ct);

            // 2) Return the retrieved stock valuation report wrapped in a Result object indicating success.
            return Result<List<StockValuationItemDto>>.Ok(rows);
        }
    }
}
