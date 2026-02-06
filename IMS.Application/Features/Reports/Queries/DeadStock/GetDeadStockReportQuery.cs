
using IMS.Application.Common.Results;
using MediatR;

namespace IMS.Application.Features.Reports.Queries.DeadStock
{
    // Query to get dead stock report, optionally filtered by warehouse and/or product
    public sealed record GetDeadStockReportQuery(
        int Days = 30,
        int? WarehouseId = null

    ) : IRequest<Result<List<DeadStockItemDto>>>;
}
