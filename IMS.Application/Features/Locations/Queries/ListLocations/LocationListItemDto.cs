
namespace IMS.Application.Features.Locations.Queries.ListLocations
{
    // DTO for returning location details in ListLocationsQuery
    public sealed record LocationListItemDto(
        int Id,
        int WarehouseId,
        string Code,
        bool IsActive
        );
}
