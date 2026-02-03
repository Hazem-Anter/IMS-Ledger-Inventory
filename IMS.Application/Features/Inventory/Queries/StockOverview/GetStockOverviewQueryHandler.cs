
using IMS.Application.Abstractions.Read;
using IMS.Application.Common.Result;
using MediatR;


namespace IMS.Application.Features.Inventory.Queries.StockOverview
{
    public sealed class GetStockOverviewQueryHandler 
        : IRequestHandler<GetStockOverviewQuery, Result<List<StockOverviewItemDto>>>
    {
        private readonly IStockReadService _read;

        public GetStockOverviewQueryHandler(IStockReadService read)
        {
            _read = read;
        }

        public async Task<Result<List<StockOverviewItemDto>>> Handle(GetStockOverviewQuery q, CancellationToken ct)
        {
            // 1) Get data from read service 
            var items = await _read.GetStockOverviewAsync(q.WarehouseId, q.ProductId, q.LowStockOnly, ct);

            // 2) Return result
            return Result<List<StockOverviewItemDto>>.Ok(items);
        }
    }
}
