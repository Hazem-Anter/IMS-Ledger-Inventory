
using FluentValidation;

namespace IMS.Application.Features.Products.Queries.GetProductById
{
    // This class defines the validation rules for the GetProductByIdQuery using FluentValidation.
    public sealed class GetProductByIdQueryValidator : AbstractValidator<GetProductByIdQuery>
    {
        public GetProductByIdQueryValidator()
        {
            RuleFor(x => x.id)
                .GreaterThan(0);
        }
    }
}
