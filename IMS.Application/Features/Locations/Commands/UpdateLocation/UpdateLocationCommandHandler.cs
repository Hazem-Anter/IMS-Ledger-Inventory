using IMS.Application.Abstractions.Persistence;
using IMS.Application.Common.Results;
using IMS.Domain.Entities;
using MediatR;

namespace IMS.Application.Features.Locations.Commands.UpdateLocation
{
    // Handler for processing the UpdateLocationCommand.
    public sealed class UpdateLocationCommandHandler : IRequestHandler<UpdateLocationCommand, Result<int>>
    {
        private readonly IRepository<Location> _locations;
        private readonly IUnitOfWork _uow;

        public UpdateLocationCommandHandler(IRepository<Location> locations, IUnitOfWork uow)
        {
            _locations = locations;
            _uow = uow;
        }

        public async Task<Result<int>> Handle(UpdateLocationCommand cmd, CancellationToken ct)
        {
            // 1) Validate that the location exists and belongs to the specified warehouse.
            // If not, return a failure result with an appropriate error message.
            var location = await _locations.GetByIdAsync(cmd.LocationId, ct);
            if (location is null || location.WarehouseId != cmd.WarehouseId)
                return Result<int>.Fail("Location not found.");


            var code = cmd.Code.Trim().ToUpperInvariant();

            // 2) Check for duplicate location code within the same warehouse, excluding the current location. 
            // If a duplicate is found, return a failure result with an appropriate error message.
            if (await _locations.AnyAsync(l =>
                    l.Id != cmd.LocationId &&
                    l.WarehouseId == cmd.WarehouseId &&
                    l.Code == code, ct))
                return Result<int>.Fail("Location code already exists in this warehouse.");

            // 3) Update the location's code and save changes to the database. 
            location.SetCode(code);

            // 4) Return a success result containing the ID of the updated location.
            await _uow.SaveChangesAsync(ct);
            return Result<int>.Ok(location.Id);
        }
    }
}
