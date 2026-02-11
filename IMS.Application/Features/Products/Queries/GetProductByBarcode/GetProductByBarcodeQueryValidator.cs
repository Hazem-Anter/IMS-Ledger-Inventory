
using FluentValidation;

namespace IMS.Application.Features.Products.Queries.GetProductByBarcode
{
    // This class defines the validation rules for the GetProductByBarcodeQuery using FluentValidation.
    public sealed class GetProductByBarcodeQueryValidator : AbstractValidator<GetProductByBarcodeQuery>
    {
        public GetProductByBarcodeQueryValidator()
        {
            RuleFor(x => x.barcode)
                .NotEmpty()
                .WithMessage("Barcode is required.")
                .MaximumLength(64);
        }
    }
}
