
using FluentValidation;

namespace IMS.Application.Features.Inventory.Commands.IssueStock
{
    // This class defines the validation rules for the IssueStockCommand using FluentValidation.
    public sealed class IssueStockCommandValidator : AbstractValidator<IssueStockCommand>
    {
        public IssueStockCommandValidator()
        {
            RuleFor(x => x.ProductId).GreaterThan(0);
            RuleFor(x => x.WarehouseId).GreaterThan(0);
            RuleFor(x => x.Quantity).GreaterThan(0);

            RuleFor(x => x.Referencetype)
                .MaximumLength(50)
                .When(x => !string.IsNullOrWhiteSpace(x.Referencetype));

            RuleFor(x => x.ReferenceId)
                .MaximumLength(100)
                .When(x => !string.IsNullOrWhiteSpace(x.ReferenceId));
        }
    }
}
