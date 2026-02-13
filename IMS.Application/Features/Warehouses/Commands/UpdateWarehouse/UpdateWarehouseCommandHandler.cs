
using IMS.Application.Abstractions.Persistence;
using IMS.Application.Common.Results;
using IMS.Domain.Entities;
using MediatR;
using System.Runtime.InteropServices;

namespace IMS.Application.Features.Warehouses.Commands.UpdateWarehouse
{
    // Handler for the UpdateWarehouseCommand, 
    // responsible for processing the command to update an existing warehouse and returning the result of the operation.
    public sealed class UpdateWarehouseCommandHandler
        : IRequestHandler<UpdateWarehouseCommand, Result<int>>
    {
        private readonly IRepository<Warehouse> _warehouse;
        private readonly IUnitOfWork _uow; 

        public UpdateWarehouseCommandHandler( IRepository<Warehouse> warehouse, IUnitOfWork uow) 
        { 
            _warehouse = warehouse;
            _uow = uow;
        }

        public async Task<Result<int>> Handle(
            UpdateWarehouseCommand cmd,
            CancellationToken ct)
        {
            // 1) Retrieve the existing warehouse by its ID. If not found, return a failure result.
            var warehouse = await _warehouse.GetByIdAsync(cmd.Id, ct);
            if (warehouse is null)
                return Result<int>.Fail("Warehouse not found.");

            // 2) Validate the new warehouse code to ensure it is unique (ignoring case).
            // If a duplicate code exists, return a failure result.
            var code = cmd.Code.Trim().ToUpperInvariant();

            if (await _warehouse.AnyAsync(w => w.Id != cmd.Id && w.Code == code, ct))
                return Result<int>.Fail("Warehouse code already exists.");

            // 3) Update the warehouse's name and code with the new values from the command.
            warehouse.SetName(cmd.Name);
            warehouse.SetCode(code);

            // 4) Save the changes to the database using the unit of work,
            // and return a success result with the warehouse ID.
            await _uow.SaveChangesAsync(ct); 
            
            return Result<int>.Ok(warehouse.Id);
        }
    }
}
