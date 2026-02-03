
namespace IMS.Application.Abstractions.Caching
{
    // Marker interface for cacheable queries
    // Used to identify queries that can be cached 
    public interface ICacheableQuery
    {
        string CacheKey { get; }
        TimeSpan? SlidingExpiration { get; }
    }
}
