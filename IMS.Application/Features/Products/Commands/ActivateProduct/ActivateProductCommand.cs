
using IMS.Application.Abstractions.Transaction;
using IMS.Application.Common.Results;
using MediatR;

namespace IMS.Application.Features.Products.Commands.ActivateProduct
{
    // This command is used to activate a product in the inventory management system.
    public sealed record ActivateProductCommand(int Id) 
        : IRequest<Result<int>>, ITransactionalCommand;
}
