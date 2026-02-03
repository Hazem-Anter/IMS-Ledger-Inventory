
using IMS.Application.Abstractions.Caching;
using MediatR;

namespace IMS.Application.Common.Behaviors
{
    // Pipeline behavior to handle caching for requests
    // This behavior intercepts requests that implement ICacheableQuery
    // and manages caching logic  for them 
    // It checks the cache before proceeding to the next handler
    // and stores the response in the cache after handling
    public sealed class CachingBehavior<TRequest, TResponse> 
        : IPipelineBehavior<TRequest, TResponse> 
        where TRequest : notnull
    {
        // Dependencies: Cache service and Cache version service
        // The cache service is used to get and set cached responses
        // The cache version service is used to manage versioning for cache keys
        private readonly ICacheService _cache;
        private readonly ICacheVersionService _versions;

        public CachingBehavior(ICacheService cashe, ICacheVersionService versions) 
        {
            _cache = cashe;
            _versions = versions;
        }

        
        public async Task<TResponse> Handle(
            TRequest request,
            RequestHandlerDelegate<TResponse> next,
            CancellationToken cancellationToken)
        {
            // 1) Check if the request is cacheable
            // If not, proceed to the next handler 
            if (request is not ICacheableQuery cacheable)
                return await next();

            // 2) Construct the final cache key
            string finalKey = cacheable.CacheKey;

            // If the request supports versioning, include the version in the cache key
            if (request is IVersionedCacheableQuery vq)
            {
                // Get the current version for the cache prefix
                var v = _versions.GetVersion(vq.CachePrefix);

                // Append the version to the cache key 
                // Format: "{CachePrefix}:v={version}:{CacheKeyWithoutVersion}"
                // E.g., "UserList:v=3:Page=1&Size=10"
                // This allows for easy invalidation of cached entries
                finalKey = $"{vq.CachePrefix}:v={v}:{vq.CacheKeyWithoutVersion}";
            }

            // 3) Attempt to retrieve the response from the cache
            // If found, return the cached response
            if (_cache.TryGetValue(finalKey, out TResponse? cached) && cached is not null)
                return cached;

            // 4) If not found in cache, proceed to the next handler
            var response = await next();

            // 5) Store the response in the cache for future requests
            _cache.Set(finalKey, response, cacheable.SlidingExpiration);

            // 6) Return the response
            return response;

        }
    }
}
