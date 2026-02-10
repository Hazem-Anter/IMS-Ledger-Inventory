
using FluentValidation;

namespace IMS.Application.Features.Products.Queries.ProductTimeline
{
    public sealed class GetProductTimelineQueryValidator : AbstractValidator<GetProductTimelineQuery>
    {
        public GetProductTimelineQueryValidator()
        {
            RuleFor(x => x.ProductId).GreaterThan(0);

            RuleFor(x => x)
                .Must(x => !x.FromUtc.HasValue || !x.ToUtc.HasValue || x.FromUtc <= x.ToUtc)
                .WithMessage("FromUtc must be <= ToUtc.");

            RuleFor(x => x.Page).GreaterThan(0);
            RuleFor(x => x.PageSize).InclusiveBetween(1, 200);

            RuleFor(x => x.WarehouseId)
                .GreaterThan(0)
                .When(x => x.WarehouseId.HasValue);
        }
    }
}
