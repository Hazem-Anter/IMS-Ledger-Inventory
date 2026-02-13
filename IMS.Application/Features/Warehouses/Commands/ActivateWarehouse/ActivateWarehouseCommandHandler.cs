using IMS.Application.Abstractions.Persistence;
using IMS.Application.Common.Results;
using IMS.Domain.Entities;
using MediatR;

namespace IMS.Application.Features.Warehouses.Commands.ActivateWarehouse
{
    // Handler for the ActivateWarehouseCommand, 
    // responsible for processing the command to activate a warehouse and returning the result of the operation.
    public sealed class ActivateWarehouseCommandHandler
        : IRequestHandler<ActivateWarehouseCommand, Result<int>>
    {
        private readonly IRepository<Warehouse> _warehouse;
        private readonly IUnitOfWork _uow; 
        
        public ActivateWarehouseCommandHandler( IRepository<Warehouse> warehouse, IUnitOfWork uow) 
        { 
            _warehouse = warehouse;
            _uow = uow; 
        }

        public async Task<Result<int>> Handle(
            ActivateWarehouseCommand cmd, CancellationToken ct)
        {
            // 1) Retrieve the warehouse entity by its ID using the reposit
            // If the warehouse is not found, return a failure result with an appropriate error message.
            var warehouse = await _warehouse.GetByIdAsync(cmd.Id, ct);
            if (warehouse is null)
                return Result<int>.Fail("Warehouse not found.");

            // 2) Activate the warehouse by calling the Activate method on the retrieved entity.
            warehouse.Activate();

            // 3) Save the changes to the database using the unit of work pattern, ensuring that the operation is atomic and consistent.
            await _uow.SaveChangesAsync(ct); 
            
            return Result<int>.Ok(warehouse.Id);
        }
    }
}
