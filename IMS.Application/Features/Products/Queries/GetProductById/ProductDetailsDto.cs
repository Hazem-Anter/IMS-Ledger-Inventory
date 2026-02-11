
namespace IMS.Application.Features.Products.Queries.GetProductById
{
    // DTO (Data Transfer Object) for returning detailed information about a product.
    public sealed record ProductDetailsDto(
        int Id,
        string Name,
        string Sku,
        string? Barcode,
        int MinStockLevel,
        bool IsActive
        );
}
