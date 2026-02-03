
using IMS.Application.Abstractions.Caching;
using IMS.Application.Common.Results;
using MediatR;

namespace IMS.Application.Features.Inventory.Commands.TransferStock
{
    public sealed record TransferStockCommand(
        int ProductId,
        int FromWarehouseId,
        int? FromLocationId,
        int ToWarehouseId,
        int? ToLocationId,
        int Quantity,
        string? ReferenceType = null,
        string? ReferenceId = null
        ) : IRequest<Result<int>>, IInvalidatesCachePrefix
    {
        public IEnumerable<string> CachePrefixesToInvalidate => new[] { "stock-overview" };
    }
}
