
using IMS.Application.Abstractions.Read;
using IMS.Application.Common.Results;
using IMS.Application.Features.Lookups.Dtos;
using MediatR;

namespace IMS.Application.Features.Lookups.Queries.WarehouseLookup
{
    // Handler for the GetWarehousesLookupQuery, responsible for processing the query and returning a list of warehouses for lookup purposes.
    public sealed class GetWarehousesLookupQueryHandler
    : IRequestHandler<GetWarehousesLookupQuery, Result<IReadOnlyList<WarehouseLookupDto>>>
    {
        private readonly IWarehouseReadService _read;

        public GetWarehousesLookupQueryHandler(IWarehouseReadService read)
        {
            _read = read;
        }

        public async Task<Result<IReadOnlyList<WarehouseLookupDto>>> Handle(
            GetWarehousesLookupQuery q,
            CancellationToken ct)
        {
            // 1) Use the read service to fetch the list of warehouses based on the ActiveOnly flag from the query.
            var items = await _read.LookupAsync(q.ActiveOnly, ct);

            // 2) Return the list of warehouses wrapped in a Result object indicating success.
            return Result<IReadOnlyList<WarehouseLookupDto>>.Ok(items);
        }
    }
}
