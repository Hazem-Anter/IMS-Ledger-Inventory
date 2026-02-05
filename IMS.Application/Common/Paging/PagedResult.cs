
namespace IMS.Application.Common.Paging
{
    // A generic record to represent a paged result set
    // Example usage: PagedResult<ProductDto> 
    // to represent a paged list of products.
    public sealed record PagedResult<T>(
        IReadOnlyList<T> Items,
        int TotalCount,
        int Page,
        int PageSize
        )
    {
        public int TotalPages => (int)Math.Ceiling(TotalCount / (double)PageSize);
    }
}
