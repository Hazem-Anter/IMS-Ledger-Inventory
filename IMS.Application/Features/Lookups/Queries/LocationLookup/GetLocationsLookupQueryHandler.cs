
using IMS.Application.Abstractions.Read;
using IMS.Application.Common.Results;
using IMS.Application.Features.Lookups.Dtos;
using MediatR;

namespace IMS.Application.Features.Lookups.Queries.LocationLookup
{
    // Handler for the GetLocationsLookupQuery, responsible for processing the query and returning a list of locations for lookup purposes.
    public sealed class GetLocationsLookupQueryHandler
    : IRequestHandler<GetLocationsLookupQuery, Result<IReadOnlyList<LocationLookupDto>>>
    {
        private readonly ILocationReadService _read;

        public GetLocationsLookupQueryHandler(ILocationReadService read)
        {
            _read = read;
        }

        public async Task<Result<IReadOnlyList<LocationLookupDto>>> Handle(
            GetLocationsLookupQuery q,
            CancellationToken ct)
        {
            // 1) Validate the WarehouseId parameter. If it's not provided or invalid, return a failure result.
            if (q.WarehouseId <= 0)
                return Result<IReadOnlyList<LocationLookupDto>>.Fail("WarehouseId is required.");

            // 2) Use the ILocationReadService to fetch the list of locations
            // based on the provided parameters (WarehouseId, Search, ActiveOnly, Take).
            var items = await _read.LookupByWarehouseAsync(
                                q.WarehouseId,
                                q.Search,
                                q.ActiveOnly,
                                q.Take,
                                ct);

            // 3) Return the fetched list of locations wrapped in a successful result.
            return Result<IReadOnlyList<LocationLookupDto>>.Ok(items);
        }
    }
}
