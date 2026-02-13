

using IMS.Application.Abstractions.Transaction;
using IMS.Application.Common.Results;
using MediatR;

namespace IMS.Application.Features.Locations.Commands.UpdateLocation
{
    // Command for updating an existing location's code within a specific warehouse.
    public sealed record UpdateLocationCommand(int WarehouseId, int LocationId, string Code)
        : IRequest<Result<int>>, ITransactionalCommand;
}
