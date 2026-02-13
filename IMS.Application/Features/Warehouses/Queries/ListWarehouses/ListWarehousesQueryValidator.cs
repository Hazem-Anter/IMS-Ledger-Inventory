
using FluentValidation;

namespace IMS.Application.Features.Warehouses.Queries.ListWarehouses
{
    // Validator for the ListWarehousesQuery to ensure that the input data is valid before processing the query.
    public sealed class ListWarehousesQueryValidator : AbstractValidator<ListWarehousesQuery>
    {
        public ListWarehousesQueryValidator() 
        {
            RuleFor(q => q.Page)
                .GreaterThan(0);

            RuleFor(q => q.PageSize)
                .GreaterThan(0)
                .InclusiveBetween(1, 100);

            RuleFor(q => q.Search)
                .MaximumLength(100);
        }
    }
}
