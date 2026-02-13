
namespace IMS.Application.Features.Locations.Queries.GetLocationById
{
    // DTO for returning location details in GetLocationByIdQuery
    public sealed record LocationDetailsDto(
    int Id,
    int WarehouseId,
    string Code,
    bool IsActive);
}
