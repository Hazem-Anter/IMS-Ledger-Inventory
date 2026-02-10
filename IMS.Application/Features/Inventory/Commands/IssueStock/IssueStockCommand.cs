
using IMS.Application.Abstractions.Caching;
using IMS.Application.Abstractions.Transaction;
using IMS.Application.Common.Results;
using MediatR;

namespace IMS.Application.Features.Inventory.Commands.IssueStock
{
    public sealed record IssueStockCommand(
        int ProductId,
        int WarehouseId,
        int? LocationId,
        int Quantity,
        string? Referencetype = null,
        string? ReferenceId = null

        ) : IRequest<Result<int>>, IInvalidatesCachePrefix, ITransactionalCommand
    {
        public IEnumerable<string> CachePrefixesToInvalidate => new[] { "stock-overview" };
    }
}
