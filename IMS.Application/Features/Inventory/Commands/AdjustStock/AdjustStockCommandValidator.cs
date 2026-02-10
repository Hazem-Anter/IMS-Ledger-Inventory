
using FluentValidation;

namespace IMS.Application.Features.Inventory.Commands.AdjustStock
{
    // This class defines the validation rules for the AdjustStockCommand using FluentValidation.
    public sealed class AdjustStockCommandValidator : AbstractValidator<AdjustStockCommand>
    {
        public AdjustStockCommandValidator()
        {
            RuleFor(x => x.ProductId).GreaterThan(0);
            RuleFor(x => x.WarehouseId).GreaterThan(0);

            RuleFor(x => x.DeltaQuantity)
                .NotEqual(0)
                .WithMessage("DeltaQuantity cannot be 0.");

            RuleFor(x => x.Reason)
                .NotEmpty()
                .MaximumLength(200);

            RuleFor(x => x.ReferenceType)
                .MaximumLength(50)
                .When(x => !string.IsNullOrWhiteSpace(x.ReferenceType));

            RuleFor(x => x.ReferenceId)
                .MaximumLength(100)
                .When(x => !string.IsNullOrWhiteSpace(x.ReferenceId));
        }
    }
}
