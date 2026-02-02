
using IMS.Application.Common.Result;
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

        ) : IRequest<Result<int>>;
}
