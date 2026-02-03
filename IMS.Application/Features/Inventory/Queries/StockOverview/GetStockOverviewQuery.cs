
using IMS.Application.Common.Result;
using MediatR;

namespace IMS.Application.Features.Inventory.Queries.StockOverview
{
    public sealed record GetStockOverviewQuery(
        int? WarehouseId = null,
        int? ProductId = null,
        bool LowStockOnly = false

    ) : IRequest<Result<List<StockOverviewItemDto>>>;
}
