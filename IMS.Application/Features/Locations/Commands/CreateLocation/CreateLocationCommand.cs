
using IMS.Application.Abstractions.Transaction;
using IMS.Application.Common.Results;
using MediatR;

namespace IMS.Application.Features.Locations.Commands.CreateLocation
{
    // Command for creating a new location within a specific warehouse.
    public sealed record CreateLocationCommand(int WarehouseId, string Code)
        : IRequest<Result<int>>, ITransactionalCommand;
}
