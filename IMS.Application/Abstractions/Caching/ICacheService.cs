
namespace IMS.Application.Abstractions.Caching
{
    // Abstraction for a caching service
    public interface ICacheService
    {
        // Tries to get a value from the cache by key
        bool TryGetValue<T>(string key, out T? value);

        // Sets a value in the cache with an optional sliding expiration 
        void Set<T>(string key, T value, TimeSpan? slidingExpiration = null);

        // Removes a value from the cache by key
        void Remove(string key);
    }
}
