
using IMS.Application.Abstractions.Transaction;
using IMS.Application.Common.Results;
using MediatR;

namespace IMS.Application.Features.Locations.Commands.ActivateLocation
{
    // Command for activating a location within a specific warehouse.
    public sealed record ActivateLocationCommand(int WarehouseId, int LocationId)
    : IRequest<Result<int>>, ITransactionalCommand;
}
