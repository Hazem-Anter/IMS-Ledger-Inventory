
using FluentValidation;

namespace IMS.Application.Features.Products.Queries.ListProducts
{
    public sealed class ListProductsQueryValidator : AbstractValidator<ListProductsQuery>
    {
        // This class defines the validation rules for the ListProductsQuery using FluentValidation.
        public ListProductsQueryValidator() 
        {
            RuleFor(x => x.Page)
                .GreaterThan(0);

            RuleFor(x => x.PageSize)
                .GreaterThan(0)
                .LessThanOrEqualTo(100);

            RuleFor(x => x.Search)
                .MaximumLength(50)
                .When(x => !string.IsNullOrWhiteSpace(x.Search));
        }
    }
}
