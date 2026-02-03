
using IMS.Application.Abstractions.Caching;
using Microsoft.Extensions.Caching.Memory;

namespace IMS.Infrastructure.Caching
{
    public sealed class MemoryCacheService : ICacheService
    {
        private readonly IMemoryCache _cache;

        public MemoryCacheService(IMemoryCache cache)
        {
            _cache = cache;
        }

        // Set a value in the cache with an optional sliding expiration.
        public void Set<T>(string key, T value, TimeSpan? slidingExpiration = null)
        {
            // 1) Create cache entry options, which is used to configure the cache entry.
            var options = new MemoryCacheEntryOptions();

            // 2) If a sliding expiration is provided, set it in the options.
            if (slidingExpiration is not null)
                options.SetSlidingExpiration(slidingExpiration.Value);

            // 3) Set the value in the cache with the specified key and options.
            _cache.Set(key, value!, options);
        }

        // Try to get the value from the cache.
        public bool TryGetValue<T>(string key, out T? value)
        {
            // If the value exists and can be cast to T, return true and set the out parameter.
            if (_cache.TryGetValue(key, out var obj) && obj is T cast)
            {
                value = cast;
                return true;
            }

            // Otherwise, return false and set the out parameter to default.
            value = default;
            return false;
        }

        // Remove a value from the cache by key.
        public void Remove(string key)
        {
            _cache.Remove(key);
        }
    }
}
