using IMS.Application.Common.Results;
using IMS.Application.Features.Lookups.Dtos;
using MediatR;

namespace IMS.Application.Features.Lookups.Queries.ProductLookup
{
    // Query object for retrieving a list of products for lookup purposes,
    // such as populating dropdowns or selection lists.
    public sealed record GetProductsLookupQuery(
        string? Search,
        bool ActiveOnly = true,
        int Take = 20
    ) : IRequest<Result<IReadOnlyList<ProductLookupDto>>>;
}
