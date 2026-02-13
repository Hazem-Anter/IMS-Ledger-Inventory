
using FluentValidation;

namespace IMS.Application.Features.Warehouses.Queries.GetWarehouseById
{
    // Validator for the GetWarehouseByIdQuery to ensure that the input data is valid before processing the query.
    public sealed class GetWarehouseByIdQueryValidator : AbstractValidator<GetWarehouseByIdQuery>
    {
        public GetWarehouseByIdQueryValidator()
        {
            RuleFor(q => q.id)
                .GreaterThan(0);
        }
    }
}
