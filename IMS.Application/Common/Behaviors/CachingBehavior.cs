
using IMS.Application.Abstractions.Caching;
using MediatR;
using Microsoft.Extensions.Logging;

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

        private readonly ILogger<CachingBehavior<TRequest, TResponse>> _logger;

        public CachingBehavior(
            ICacheService cashe,
            ICacheVersionService versions,
            ILogger<CachingBehavior<TRequest, TResponse>> logger) 
        {
            _cache = cashe;
            _versions = versions;
            _logger = logger;
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

            //#2.1# Log the cache key being checked 
            _logger.LogInformation("[CACHE] Checking key: {Key}", finalKey);

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

            // 3) Check if the response is already cached 
            // If found, return the cached response
            // Log cache hit if found in cache 
            if (_cache.TryGetValue(finalKey, out TResponse? cached) && cached is not null)
            {
                _logger.LogInformation("[CACHE HIT] {Key}", finalKey);
                return cached;
            }
            // Log cache miss if not found in cache
            _logger.LogInformation("[CACHE MISS] {Key}", finalKey);

            // 4) If not found in cache, proceed to the next handler
            var response = await next();

            // 5) Store the response in the cache for future requests
            // Log cache store action
            _cache.Set(finalKey, response, cacheable.SlidingExpiration);
            _logger.LogInformation("[CACHE STORE] {Key}", finalKey);

            // 6) Return the response
            return response;

        }
    }
}
