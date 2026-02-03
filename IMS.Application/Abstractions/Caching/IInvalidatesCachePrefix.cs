
namespace IMS.Application.Abstractions.Caching
{
    // Interface for requests that invalidate specific cache prefixes
    // when they are processed 
    // This is useful for commands that modify data
    // and need to ensure that related cache entries are invalidated
    // Example:
    // A command implementing this interface might specify:
    // - CachePrefixesToInvalidate: ["user", "order"]
    // When the command is executed, all cache entries associated
    // with the "user" and "order" prefixes will be invalidated
    // This helps maintain cache consistency and ensures that
    // stale data is not served from the cache
    public interface IInvalidatesCachePrefix
    {
        // The collection of cache prefixes to invalidate
        IEnumerable<string> CachePrefixesToInvalidate { get; }
    }
}
