
using IMS.Application.Abstractions.Transaction;
using IMS.Application.Common.Results;
using MediatR;

namespace IMS.Application.Features.Warehouses.Commands.UpdateWarehouse
{
    // Command for updating an existing warehouse.
    public sealed record UpdateWarehouseCommand(
        int Id,
        string Name,
        string Code
        ) : IRequest<Result<int>>, ITransactionalCommand;
}
