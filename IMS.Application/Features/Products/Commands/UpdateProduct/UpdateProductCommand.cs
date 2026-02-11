
using IMS.Application.Abstractions.Transaction;
using IMS.Application.Common.Results;
using MediatR;

namespace IMS.Application.Features.Products.Commands.UpdateProduct
{
    // This record represents a command to update an existing product's details.
    public sealed record UpdateProductCommand(
        int Id,
        string Name,
        string Sku,
        string? Barcode,
        int MinStockLevel
        ) : IRequest<Result<int>>, ITransactionalCommand;
}
