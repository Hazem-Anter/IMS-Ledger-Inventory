
namespace IMS.Application.Features.Lookups.Dtos
{
    // DTO for product lookup, containing essential information for identifying a product.
    public sealed record ProductLookupDto(
        int Id,
        string Name,
        string Sku,
        string? Barcode
    );
}
