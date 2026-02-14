
using IMS.Application.Common.Paging;
using IMS.Application.Features.Lookups.Dtos;
using IMS.Application.Features.Products.Queries.GetProductById;

namespace IMS.Application.Abstractions.Read
{
    public interface IProductReadService
    {
        Task<ProductDetailsDto?> GetByIdAsync(int productId, CancellationToken ct = default);

        Task<ProductDetailsDto?> GetByBarcode(string barcode, CancellationToken ct = default);

        Task<PagedResult<ProductDetailsDto>> ListAsync(
            string? search,
            bool? isActive,
            int page,
            int pageSize,
            CancellationToken ct = default);

        // Lookup method for products, allowing filtering by search term and active status,
        // and limiting the number of results returned.
        Task<IReadOnlyList<ProductLookupDto>> LookupAsync(
            string? search,
            bool activeOnly,
            int take,
            CancellationToken ct = default);
    }
}
