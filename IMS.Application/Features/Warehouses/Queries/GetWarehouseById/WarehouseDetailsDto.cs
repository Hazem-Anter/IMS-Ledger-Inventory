
namespace IMS.Application.Features.Warehouses.Queries.GetWarehouseById
{
    // DTO for returning warehouse details by ID
    public sealed record WarehouseDetailsDto(
        int Id,
        string Name,
        string Code,
        bool IsActive
        );
}
