
using FluentValidation;

namespace IMS.Application.Features.Reports.Queries.StockMovements
{
    // This validator class is responsible for validating the GetStockMovementsQuery request before it is processed by the handler.
    public sealed class GetStockMovementsQueryValidator : AbstractValidator<GetStockMovementsQuery>
    {
        public GetStockMovementsQueryValidator()
        {
            RuleFor(x => x.FromUtc)
                .LessThanOrEqualTo(x => x.ToUtc)
                .WithMessage("FromUtc must be <= ToUtc.");

            RuleFor(x => x.Page).GreaterThan(0);
            RuleFor(x => x.PageSize).InclusiveBetween(1, 200);

            RuleFor(x => x.WarehouseId)
                .GreaterThan(0)
                .When(x => x.WarehouseId.HasValue);

            RuleFor(x => x.ProductId)
                .GreaterThan(0)
                .When(x => x.ProductId.HasValue);
        }
    }
}
