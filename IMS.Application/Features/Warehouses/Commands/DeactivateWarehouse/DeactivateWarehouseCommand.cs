using IMS.Application.Abstractions.Transaction;
using IMS.Application.Common.Results;
using MediatR;

namespace IMS.Application.Features.Warehouses.Commands.DeactivateWarehouse
{
    // Command for deactivating a warehouse, 
    // containing the necessary information such as the warehouse ID.
    public sealed record DeactivateWarehouseCommand(int Id)
        : IRequest<Result<int>>, ITransactionalCommand;
}
