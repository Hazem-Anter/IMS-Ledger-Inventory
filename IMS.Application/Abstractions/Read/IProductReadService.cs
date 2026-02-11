
using IMS.Application.Common.Paging;
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
    }
}
