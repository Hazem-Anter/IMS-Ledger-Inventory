
using IMS.Application.Common.Result;
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
        ) : IRequest<Result<int>>;
}
