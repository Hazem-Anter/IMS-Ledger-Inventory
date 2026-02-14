
namespace IMS.Application.Features.Lookups.Dtos
{
    // DTO for warehouse lookup, containing essential information for identifying a warehouse.
    public sealed record WarehouseLookupDto(
        int Id,
        string Name,
        string Code
    );
}
