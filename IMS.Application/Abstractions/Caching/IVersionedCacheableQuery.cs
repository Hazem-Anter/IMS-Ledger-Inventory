
namespace IMS.Application.Abstractions.Caching
{
    // Interface for cacheable queries that support versioning
    // This allows for efficient cache invalidation by versioning cache keys    
    // Example:
    // A query implementing this interface might have: 
    // - CachePrefix: "user"
    // - CacheKeyWithoutVersion: "user:123"
    // The full cache key would then be "user:123:v1"
    // Incrementing the version for the "user" prefix would invalidate all related cache entries
    // without needing to track each individual key
    // This is particularly useful in scenarios where data changes frequently
    public interface IVersionedCacheableQuery : ICacheableQuery
    {
        // The prefix used for versioning the cache key
        // For example, "user" for user-related cache entries
        string CachePrefix { get; }

        // The cache key without the version suffix
        // For example, "user:123" for a specific user
        string CacheKeyWithoutVersion { get; }
    }
}
