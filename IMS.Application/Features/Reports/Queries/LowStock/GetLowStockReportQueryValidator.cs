
using FluentValidation;

namespace IMS.Application.Features.Reports.Queries.LowStock
{
    // This class defines the validation rules for the GetLowStockReportQuery using FluentValidation.
    public sealed class GetLowStockReportQueryValidator : AbstractValidator<GetLowStockReportQuery>
    {
        public GetLowStockReportQueryValidator()
        {
            RuleFor(x => x.WarehouseId)
                .GreaterThan(0)
                .When(x => x.WarehouseId.HasValue);

            RuleFor(x => x.ProductId)
                .GreaterThan(0)
                .When(x => x.ProductId.HasValue);
        }
    }
}
