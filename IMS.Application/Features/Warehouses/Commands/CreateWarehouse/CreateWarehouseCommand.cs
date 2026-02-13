
using IMS.Application.Abstractions.Transaction;
using IMS.Application.Common.Results;
using MediatR;

namespace IMS.Application.Features.Warehouses.Commands.CreateWarehouse
{
    // Command for creating a new warehouse,
    // containing the necessary information such as the warehouse name and code.
    public sealed record CreateWarehouseCommand(
        string Name,
        string Code
        ) : IRequest<Result<int>>, ITransactionalCommand;
}
