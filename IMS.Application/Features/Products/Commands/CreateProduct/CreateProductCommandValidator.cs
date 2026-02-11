
using FluentValidation;

namespace IMS.Application.Features.Products.Commands.CreateProduct
{
    // Validator for CreateProductCommand to ensure that the input data meets the required criteria before processing
    public sealed class CreateProductCommandValidator : AbstractValidator<CreateProductCommand>
    {
        public CreateProductCommandValidator()
        {
            RuleFor(x => x.Name).NotEmpty().MaximumLength(200);
            RuleFor(x => x.Sku).NotEmpty().MaximumLength(64);
            RuleFor(x => x.Barcode).MaximumLength(64);
            RuleFor(x => x.MinStockLevel).GreaterThanOrEqualTo(0);
        }
    }
}
