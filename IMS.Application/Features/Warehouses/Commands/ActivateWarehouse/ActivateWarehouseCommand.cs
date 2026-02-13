using IMS.Application.Abstractions.Transaction;
using IMS.Application.Common.Results;
using MediatR;

namespace IMS.Application.Features.Warehouses.Commands.ActivateWarehouse
{
    // Command for activating a warehouse, 
    // containing the necessary information such as the warehouse ID.
    public sealed record ActivateWarehouseCommand(int Id)
        : IRequest<Result<int>>, ITransactionalCommand;
}
