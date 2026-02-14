
using IMS.Application.Common.Results;
using IMS.Application.Features.Lookups.Dtos;
using MediatR;

namespace IMS.Application.Features.Lookups.Queries.LocationLookup
{
    // Query object for retrieving a list of locations for lookup purposes,
    // such as populating dropdowns or selection lists. It includes parameters for filtering by warehouse,
    // search term, active status, and the number of results to return.
    public sealed record GetLocationsLookupQuery(
        int WarehouseId,
        string? Search,
        bool ActiveOnly = true,
        int Take = 50
    ) : IRequest<Result<IReadOnlyList<LocationLookupDto>>>;
}
