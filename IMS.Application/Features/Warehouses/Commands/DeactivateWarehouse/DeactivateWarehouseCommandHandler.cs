
using IMS.Application.Abstractions.Persistence;
using IMS.Application.Common.Results;
using IMS.Domain.Entities;
using MediatR;

namespace IMS.Application.Features.Warehouses.Commands.DeactivateWarehouse
{
    // Handler for the DeactivateWarehouseCommand, 
    // responsible for processing the command to deactivate a warehouse and returning the result of the operation.
    public sealed class DeactivateWarehouseCommandHandler
    : IRequestHandler<DeactivateWarehouseCommand, Result<int>>
    {
        private readonly IRepository<Warehouse> _warehouses;
        private readonly IUnitOfWork _uow;

        public DeactivateWarehouseCommandHandler(IRepository<Warehouse> warehouses, IUnitOfWork uow)
        {
            _warehouses = warehouses;
            _uow = uow;
        }

        public async Task<Result<int>> Handle(DeactivateWarehouseCommand cmd, CancellationToken ct)
        {
            // 1) Retrieve the warehouse by its ID using the repository.
            // If the warehouse is not found, return a failure result.
            var warehouse = await _warehouses.GetByIdAsync(cmd.Id, ct);
            if (warehouse is null) return Result<int>.Fail("Warehouse not found.");

            // 2) Deactivate the warehouse by calling the Deactivate method on the entity.
            warehouse.Deactivate();

            // 3) Save the changes to the database using the unit of work.
            // return a success result containing the ID of the deactivated warehouse.
            await _uow.SaveChangesAsync(ct);

            return Result<int>.Ok(warehouse.Id);
        }
    }
}
