
using IMS.Application.Abstractions.Persistence;
using IMS.Application.Common.Results;
using IMS.Domain.Entities;
using MediatR;

namespace IMS.Application.Features.Locations.Commands.CreateLocation
{
    // Handler for processing the CreateLocationCommand.
    public sealed class CreateLocationCommandHandler : IRequestHandler<CreateLocationCommand, Result<int>>
    {
        private readonly IRepository<Warehouse> _warehouses;
        private readonly IRepository<Location> _locations;
        private readonly IUnitOfWork _uow;

        public CreateLocationCommandHandler(
            IRepository<Warehouse> warehouses,
            IRepository<Location> locations,
            IUnitOfWork uow)
        {
            _warehouses = warehouses;
            _locations = locations;
            _uow = uow;
        }

        public async Task<Result<int>> Handle(CreateLocationCommand cmd, CancellationToken ct)
        {
            // 1) Validate warehouse exists , if not return failure result
            if (!await _warehouses.AnyAsync(w => w.Id == cmd.WarehouseId, ct))
                return Result<int>.Fail("Warehouse not found.");

            
            var code = cmd.Code.Trim().ToUpperInvariant();

            // 2) Validate location code is unique within the warehouse, if not return failure result
            if (await _locations.AnyAsync(l => l.WarehouseId == cmd.WarehouseId && l.Code == code, ct))
                return Result<int>.Fail("Location code already exists in this warehouse.");

            // 3) Create new location entity, save to database and return success result with new location id
            var location = new Location(cmd.WarehouseId, code);

            await _locations.AddAsync(location, ct);
            await _uow.SaveChangesAsync(ct);

            return Result<int>.Ok(location.Id);
        }
    }
}
