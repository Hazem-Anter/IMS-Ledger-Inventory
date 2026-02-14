
namespace IMS.Application.Features.Lookups.Dtos
{
    // DTO for location lookup, containing essential information for identifying a location.
    public sealed record LocationLookupDto(
        int Id,
        string Code
    );
}
