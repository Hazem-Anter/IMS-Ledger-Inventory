
using IMS.Application.Abstractions.Transaction;
using IMS.Application.Common.Results;
using MediatR;

namespace IMS.Application.Features.Products.Commands.CreateProduct
{
    // Command to create a new product in the inventory system.
    public sealed record CreateProductCommand(
        string Name,
        string Sku,
        string? Barcode,
        int MinStockLevel
        ) : IRequest<Result<int>>, ITransactionalCommand;
}
