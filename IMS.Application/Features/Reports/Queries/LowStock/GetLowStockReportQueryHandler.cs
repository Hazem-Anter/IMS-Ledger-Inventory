
using IMS.Application.Abstractions.Read;
using IMS.Application.Common.Results;
using MediatR;

namespace IMS.Application.Features.Reports.Queries.LowStock
{
    public sealed class GetLowStockReportQueryHandler 
        : IRequestHandler<GetLowStockReportQuery, Result<List<LowStockItemDto>>>
    {
        private readonly IStockReadService _read;
        public GetLowStockReportQueryHandler(IStockReadService read)
        {
            _read = read;
        }

        // Handles the GetLowStockReportQuery by calling the read service to get the low stock report data
        public async Task<Result<List<LowStockItemDto>>> Handle(
            GetLowStockReportQuery q,
            CancellationToken ct)
        {
            // 1) Call the read service to get the low stock report data based on the query parameters
            var rows = await _read.GetLowStockReportAsync(
             q.WarehouseId,
             q.ProductId,
             ct);

            // 2) Return the result wrapped in a Result object indicating success
            return Result<List<LowStockItemDto>>.Ok(rows);
        }
    
    }
}
