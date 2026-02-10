
using FluentValidation;

namespace IMS.Application.Features.Inventory.Commands.ReceiveStock
{
    // This class defines the validation rules for the ReceiveStockCommand using FluentValidation.
    public sealed class ReceiveStockCommandValidator : AbstractValidator<ReceiveStockCommand>
    {
        public ReceiveStockCommandValidator()
        {
            RuleFor(x => x.ProductId).GreaterThan(0);
            RuleFor(x => x.WarehouseId).GreaterThan(0);
            RuleFor(x => x.Quantity).GreaterThan(0);

            RuleFor(x => x.UnitCost)
                .GreaterThanOrEqualTo(0)
                .When(x => x.UnitCost.HasValue);

            RuleFor(x => x.ReferenceType)
                .MaximumLength(50)
                .When(x => !string.IsNullOrWhiteSpace(x.ReferenceType));

            RuleFor(x => x.ReferenceId)
                .MaximumLength(100)
                .When(x => !string.IsNullOrWhiteSpace(x.ReferenceId));
        }
    }
}
