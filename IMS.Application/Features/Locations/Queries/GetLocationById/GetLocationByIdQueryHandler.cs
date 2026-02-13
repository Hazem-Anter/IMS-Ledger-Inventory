
using IMS.Application.Abstractions.Read;
using IMS.Application.Common.Results;
using MediatR;

namespace IMS.Application.Features.Locations.Queries.GetLocationById
{
    // Handler for processing the GetLocationByIdQuery.
    // It uses the ILocationReadService to fetch the location details.
    public sealed class GetLocationByIdQueryHandler
    : IRequestHandler<GetLocationByIdQuery, Result<LocationDetailsDto>>
    {
        private readonly ILocationReadService _read;

        public GetLocationByIdQueryHandler(ILocationReadService read)
        {
            _read = read;
        }

        public async Task<Result<LocationDetailsDto>> Handle(GetLocationByIdQuery q, CancellationToken ct)
        {
            // 1) Use the read service to get the location details by warehouse and location ID.
            var dto = await _read.GetByIdAsync(q.WarehouseId, q.LocationId, ct);

            // 2) If the location is not found, return a failure result;
            // otherwise, return a success result with the location details.
            return dto is null
                ? Result<LocationDetailsDto>.Fail("Location not found.")
                : Result<LocationDetailsDto>.Ok(dto);
        }
    }
}
