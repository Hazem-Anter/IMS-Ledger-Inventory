
using FluentValidation;

namespace IMS.Application.Features.Products.Commands.UpdateProduct
{
    // This class defines the validation rules for the UpdateProductCommand using FluentValidation.
    public sealed class UpdateProductCommandValidator : AbstractValidator<UpdateProductCommand>
    {
        public UpdateProductCommandValidator()
        {
            RuleFor(x => x.Id).GreaterThan(0);
            RuleFor(x => x.Name).NotEmpty().MaximumLength(200);
            RuleFor(x => x.Sku).NotEmpty().MaximumLength(64);
            RuleFor(x => x.Barcode).MaximumLength(64);
            RuleFor(x => x.MinStockLevel).GreaterThanOrEqualTo(0);
        }
    }
}
