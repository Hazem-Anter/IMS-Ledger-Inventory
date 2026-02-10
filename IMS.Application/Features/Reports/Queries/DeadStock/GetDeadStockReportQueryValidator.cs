
using FluentValidation;

namespace IMS.Application.Features.Reports.Queries.DeadStock
{
    // This class defines the validation rules for the GetDeadStockReportQuery using FluentValidation.
    public sealed class GetDeadStockReportQueryValidator : AbstractValidator<GetDeadStockReportQuery>
    {
        public GetDeadStockReportQueryValidator()
        {
            RuleFor(x => x.Days).InclusiveBetween(1, 3650);

            RuleFor(x => x.WarehouseId)
                .GreaterThan(0)
                .When(x => x.WarehouseId.HasValue);
        }
    }
}
