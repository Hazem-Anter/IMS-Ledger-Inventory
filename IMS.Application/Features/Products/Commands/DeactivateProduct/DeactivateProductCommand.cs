using IMS.Application.Abstractions.Transaction;
using IMS.Application.Common.Results;
using MediatR;

namespace IMS.Application.Features.Products.Commands.DeactivateProduct
{
    // This command is used to deactivate a product in the inventory management system.
    public sealed record DeactivateProductCommand(int Id) 
        : IRequest<Result<int>>, ITransactionalCommand;
}
