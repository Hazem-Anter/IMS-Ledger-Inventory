
using IMS.Application.Common.Results;
using MediatR;

namespace IMS.Application.Features.Reports.Queries.StockValuation
{
    // This record defines the structure of the query that will be sent to the MediatR pipeline to request a stock valuation report.
    public sealed record GetStockValuationReportQuery(
        StockValuationMode Mode = StockValuationMode.Fifo,
        int? WarehouseId = null,
        int? ProductId = null

    ) : IRequest<Result<List<StockValuationItemDto>>>;
}
