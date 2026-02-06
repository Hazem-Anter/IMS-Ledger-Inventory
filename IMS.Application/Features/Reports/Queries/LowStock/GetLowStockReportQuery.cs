
using IMS.Application.Common.Results;
using MediatR;

namespace IMS.Application.Features.Reports.Queries.LowStock
{
    // Query to get low stock report, optionally filtered by warehouse and/or product
    public sealed record GetLowStockReportQuery(
        int? WarehouseId = null,
        int? ProductId = null

        ) : IRequest<Result<List<LowStockItemDto>>>;
}
