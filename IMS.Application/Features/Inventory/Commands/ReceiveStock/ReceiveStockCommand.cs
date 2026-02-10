using IMS.Application.Abstractions.Caching;
using IMS.Application.Abstractions.Transaction;
using IMS.Application.Common.Results;
using MediatR;

namespace IMS.Application.Features.Inventory.Commands.ReceiveStock
{
    public sealed record ReceiveStockCommand(
        int ProductId,
        int WarehouseId,
        int? LocationId,
        int Quantity,
        decimal? UnitCost = null,
        string? ReferenceType = null,
        string? ReferenceId = null

        ) : IRequest<Result<int>>, IInvalidatesCachePrefix, ITransactionalCommand
    {
        public IEnumerable<string> CachePrefixesToInvalidate => new[] { "stock-overview" };
    }
    // returns the new StockTransaction Id
    // IRequest take a single type parameter for the response type 

}
