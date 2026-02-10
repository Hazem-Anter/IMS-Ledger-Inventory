
using FluentValidation;

namespace IMS.Application.Features.Reports.Queries.StockValuation
{
    // This validator class is responsible for validating the GetStockValuationReportQuery, which is a query object used to request a stock valuation report.
    public sealed class GetStockValuationReportQueryValidator : AbstractValidator<GetStockValuationReportQuery>
    {
        public GetStockValuationReportQueryValidator()
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
