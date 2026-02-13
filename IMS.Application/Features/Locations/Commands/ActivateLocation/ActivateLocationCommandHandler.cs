using IMS.Application.Abstractions.Persistence;
using IMS.Application.Common.Results;
using IMS.Domain.Entities;
using MediatR;

namespace IMS.Application.Features.Locations.Commands.ActivateLocation
{
    public sealed class ActivateLocationCommandHandler
    : IRequestHandler<ActivateLocationCommand, Result<int>>
    {
        private readonly IRepository<Location> _locations;
        private readonly IUnitOfWork _uow;

        public ActivateLocationCommandHandler(IRepository<Location> locations, IUnitOfWork uow)
        {
            _locations = locations;
            _uow = uow;
        }

        public async Task<Result<int>> Handle(ActivateLocationCommand cmd, CancellationToken ct)
        {
            // 1) Check if the location exists and belongs to the specified warehouse.
            // If not, return a failure result with an appropriate error message.
            var location = await _locations.GetByIdAsync(cmd.LocationId, ct);
            if (location is null || location.WarehouseId != cmd.WarehouseId)
                return Result<int>.Fail("Location not found.");

            // 2) Activate the location by calling its Activate method,
            // then save the changes to the database using the unit of work.
            location.Activate();
            await _uow.SaveChangesAsync(ct);

            return Result<int>.Ok(location.Id);
        }
    }
}
