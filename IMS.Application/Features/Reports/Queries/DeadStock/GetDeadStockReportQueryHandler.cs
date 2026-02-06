
using IMS.Application.Abstractions.Read;
using IMS.Application.Common.Results;
using MediatR;

namespace IMS.Application.Features.Reports.Queries.DeadStock
{
    public sealed class GetDeadStockReportQueryHandler
    : IRequestHandler<GetDeadStockReportQuery, Result<List<DeadStockItemDto>>>
    {
        private readonly IStockReadService _read;

        public GetDeadStockReportQueryHandler(IStockReadService read)
        {
            _read = read;
        }

        // Get items that have not moved for the specified number of days, optionally filtered by warehouse
        public async Task<Result<List<DeadStockItemDto>>> Handle(
            GetDeadStockReportQuery q,
            CancellationToken ct)
        {
            // 1) Validate input
            if (q.Days <= 0)
                return Result<List<DeadStockItemDto>>.Fail("Days must be greater than zero.");

            // 2) Get data from read service
            var rows = await _read.GetDeadStockReportAsync(q.Days, q.WarehouseId, ct);

            // 3) Return result
            return Result<List<DeadStockItemDto>>.Ok(rows);
        }
    }
}
