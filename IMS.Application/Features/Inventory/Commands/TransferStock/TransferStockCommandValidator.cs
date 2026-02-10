
using FluentValidation;

namespace IMS.Application.Features.Inventory.Commands.TransferStock
{
    // This class defines the validation rules for the TransferStockCommand using FluentValidation. 
    public sealed class TransferStockCommandValidator : AbstractValidator<TransferStockCommand>
    {
        public TransferStockCommandValidator() 
        {
            RuleFor(x => x.ProductId).GreaterThan(0);
            RuleFor(x => x.FromWarehouseId).GreaterThan(0);
            RuleFor(x => x.ToWarehouseId).GreaterThan(0);
            RuleFor(x => x.Quantity).GreaterThan(0);

            RuleFor(x => x)
                .Must(x => x.FromWarehouseId != x.ToWarehouseId
                        || x.FromLocationId != x.ToLocationId)
                .WithMessage("Transfer source and destination cannot be the same.");

            RuleFor(x => x.ReferenceType)
                .MaximumLength(50)
                .When(x => !string.IsNullOrWhiteSpace(x.ReferenceType));

            RuleFor(x => x.ReferenceId)
                .MaximumLength(100)
                .When(x => !string.IsNullOrWhiteSpace(x.ReferenceId));
        }
    }
}
