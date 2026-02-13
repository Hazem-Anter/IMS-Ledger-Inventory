
namespace IMS.Application.Features.Warehouses.Queries.ListWarehouses
{
    // used in search results and dropdowns, etc. for listing warehouses
    public sealed record WarehouseListItemDto(
        int Id,
        string Name,
        string Code,
        bool IsActive
        );
}
