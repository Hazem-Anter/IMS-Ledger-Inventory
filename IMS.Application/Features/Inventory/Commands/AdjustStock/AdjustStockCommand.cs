
using IMS.Application.Abstractions.Caching;
using IMS.Application.Common.Results;
using MediatR;

namespace IMS.Application.Features.Inventory.Commands.AdjustStock
{
    public sealed record AdjustStockCommand(
        int ProductId,
        int WarehouseId,
        int? LocationId,
        int DeltaQuantity,          // can be + or -
        string Reason,              // required for auditing
        string? ReferenceType = null,
        string? ReferenceId = null

        ) : IRequest<Result<int>>, IInvalidatesCachePrefix
    {
        public IEnumerable<string> CachePrefixesToInvalidate => new[] { "stock-overview" };
    }

}
