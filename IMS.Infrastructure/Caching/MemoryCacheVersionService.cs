
using IMS.Application.Abstractions.Caching;
using Microsoft.Extensions.Caching.Memory;

namespace IMS.Infrastructure.Caching
{
    // Implementation of ICacheVersionService using IMemoryCache
    // This service manages cache versions based on prefixes
    public sealed class MemoryCacheVersionService : ICacheVersionService
    {
        // Dependency: In-memory cache
        private readonly IMemoryCache _cache;

        public MemoryCacheVersionService(IMemoryCache cache)
        {
            _cache = cache;
        }

        // Constructs the cache key for storing the version of a given prefix
        // Format: "cache-version:{prefix}"
        // E.g., "cache-version:user"
        // This ensures unique keys for each prefix's version
        private static string Key(string prefix) => $"cache-version:{prefix}";

        // Gets the current version number for the specified prefix
        // If no version exists, it initializes it to 1
        public int GetVersion(string prefix)
        {
            // Retrieve the version from the cache or create it if it doesn't exist
            // The initial version is set to 1
            // This allows for easy version management and invalidation
            // E.g., first call to GetVersion("user") returns 1
            // Subsequent calls return the current version
            return _cache.GetOrCreate(Key(prefix), _ => 1);
        }

        // Increments the version number for the specified prefix
        // This invalidates all cache entries associated with that prefix
        public void Increment(string prefix)
        {
            // 1) Construct the cache key for the prefix version
            var key = Key(prefix);

            // 2) Retrieve the current version
            var current = GetVersion(prefix);

            // 3) Increment the version and update the cache
            _cache.Set(key, current + 1);
        }
    }
}
