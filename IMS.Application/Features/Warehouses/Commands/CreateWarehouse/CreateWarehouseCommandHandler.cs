
using IMS.Application.Abstractions.Persistence;
using IMS.Application.Common.Results;
using IMS.Domain.Entities;
using MediatR;
using System.ComponentModel.DataAnnotations;

namespace IMS.Application.Features.Warehouses.Commands.CreateWarehouse
{
    // Handler for the CreateWarehouseCommand,
    // responsible for processing the command to create a new warehouse and returning the result of the operation.
    public sealed class CreateWarehouseCommandHandler
        : IRequestHandler<CreateWarehouseCommand, Result<int>>
    {
        private readonly IRepository<Warehouse> _warehouse;
        private readonly IUnitOfWork _uow;

        public CreateWarehouseCommandHandler(
            IRepository<Warehouse> warehouse, IUnitOfWork uow)
        {
            _warehouse = warehouse;
            _uow = uow;
        }

        public async Task<Result<int>> Handle(
            CreateWarehouseCommand cmd, CancellationToken ct)
        {
            // 1) trim and convert the warehouse code to uppercase \
            // to ensure consistency and avoid case sensitivity issues when checking for duplicates.
            var code = cmd.Code.Trim().ToUpperInvariant();

            // 2) Check if a warehouse with the same code already exists in the repository.
            if (await _warehouse.AnyAsync(w => w.Code == code, ct))
                return Result<int>.Fail("Warehouse code already exists.");

            // 3) If the code is unique, create a new Warehouse entity using the provided name and code
            var warehouse = new Warehouse(cmd.Name, cmd.Code);
            await _warehouse.AddAsync(warehouse, ct);

            // 4) Save the changes to the database using the unit of work pattern,
            // ensuring that all operations are executed within a transaction.
            await _uow.SaveChangesAsync(ct);

            return Result<int>.Ok(warehouse.Id);
        }
    }
}
