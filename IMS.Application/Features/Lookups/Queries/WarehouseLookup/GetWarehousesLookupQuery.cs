
using IMS.Application.Common.Results;
using IMS.Application.Features.Lookups.Dtos;
using MediatR;

namespace IMS.Application.Features.Lookups.Queries.WarehouseLookup
{
    // Query object for retrieving a list of warehouses for lookup purposes,
    // such as populating dropdowns or selection lists.
    public sealed record GetWarehousesLookupQuery(
        bool ActiveOnly = true

    ) : IRequest<Result<IReadOnlyList<WarehouseLookupDto>>>;
}
