
using IMS.Application.Abstractions.Read;
using IMS.Application.Common.Results;
using IMS.Application.Features.Lookups.Dtos;
using MediatR;

namespace IMS.Application.Features.Lookups.Queries.ProductLookup
{
    // Handler for the GetProductsLookupQuery, responsible for processing the query and returning a list of products for lookup purposes.
    public sealed class GetProductsLookupQueryHandler
    : IRequestHandler<GetProductsLookupQuery, Result<IReadOnlyList<ProductLookupDto>>>
    {
        private readonly IProductReadService _read;

        public GetProductsLookupQueryHandler(IProductReadService read)
        {
            _read = read;
        }

        public async Task<Result<IReadOnlyList<ProductLookupDto>>> Handle(
            GetProductsLookupQuery q,
            CancellationToken ct)
        {
            // 1) Use the injected IProductReadService to perform the lookup based on the query parameters
            // (search term, active only filter, and take limit).
            var items = await _read.LookupAsync(q.Search, q.ActiveOnly, q.Take, ct);

            // 2) Return the results wrapped in a Result object, indicating success and containing the list of ProductLookupDto items.
            return Result<IReadOnlyList<ProductLookupDto>>.Ok(items);
        }
    }
}
