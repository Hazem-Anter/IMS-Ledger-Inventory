
using IMS.Application.Common.Results;
using MediatR;

namespace IMS.Application.Features.Locations.Queries.GetLocationById
{
    // Query for retrieving details of a specific location by its ID within a warehouse.
    public sealed record GetLocationByIdQuery(int WarehouseId, int LocationId)
        : IRequest<Result<LocationDetailsDto>>;
}
