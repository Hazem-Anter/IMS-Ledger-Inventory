
namespace IMS.Application.Abstractions.Caching
{
    // Marker interface for commands that invalidate cache entries 
    public interface ICacheInvalidatorCommand
    {
        // Gets the cache keys to invalidate when the command is executed
        IEnumerable<string> CacheKeysToInvalidate { get; }
    }
}
