
using IMS.Application.Abstractions.Transaction;
using IMS.Application.Common.Results;
using MediatR;

namespace IMS.Application.Features.Locations.Commands.DeactivateLocation
{
    // Command for deactivating a location within a specific warehouse. 
    public sealed record DeactivateLocationCommand(int WarehouseId, int LocationId)
    : IRequest<Result<int>>, ITransactionalCommand;
}
