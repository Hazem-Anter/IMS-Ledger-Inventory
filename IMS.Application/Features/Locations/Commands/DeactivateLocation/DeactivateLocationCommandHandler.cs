

using IMS.Application.Abstractions.Persistence;
using IMS.Application.Common.Results;
using IMS.Domain.Entities;
using MediatR;

namespace IMS.Application.Features.Locations.Commands.DeactivateLocation
{
    // Handler for processing the DeactivateLocationCommand,
    // which deactivates a location if it is not used in any stock transactions.
    public sealed class DeactivateLocationCommandHandler
    : IRequestHandler<DeactivateLocationCommand, Result<int>>
    {
        private readonly IRepository<Location> _locations;
        private readonly IRepository<StockTransaction> _tx;
        private readonly IUnitOfWork _uow;

        public DeactivateLocationCommandHandler(
            IRepository<Location> locations,
            IRepository<StockTransaction> tx,
            IUnitOfWork uow)
        {
            _locations = locations;
            _tx = tx;
            _uow = uow;
        }

        public async Task<Result<int>> Handle(DeactivateLocationCommand cmd, CancellationToken ct)
        {
            // 1) Check if the location exists and belongs to the specified warehouse
            // If not found, return a failure result
            var location = await _locations.GetByIdAsync(cmd.LocationId, ct);
            if (location is null || location.WarehouseId != cmd.WarehouseId)
                return Result<int>.Fail("Location not found.");

            // 2) Business rule: A location cannot be deactivated if it has been used in any stock transactions
            // Check if there are any stock transactions associated with the location
            // If there are, return a failure result indicating that the location cannot be deactivated
            var used = await _tx.AnyAsync(t =>
                t.WarehouseId == cmd.WarehouseId &&
                t.LocationId == cmd.LocationId, ct);

            if (used)
                return Result<int>.Fail("Cannot deactivate a location that has stock transactions.");

            // 3) If the location is valid and not used in any transactions, proceed to deactivate it
            // Call the Deactivate method on the location entity to mark it as inactive and save the changes to the database
            location.Deactivate();
            await _uow.SaveChangesAsync(ct);

            // 4) Return a success result with the ID of the deactivated location
            return Result<int>.Ok(location.Id);
        }
    }
}
