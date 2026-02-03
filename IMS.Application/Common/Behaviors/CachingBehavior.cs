
using IMS.Application.Abstractions.Caching;
using MediatR;

namespace IMS.Application.Common.Behaviors
{
    // Caching behavior for MediatR pipeline 
    // Caches responses for requests that implement ICacheableQuery
    // Uses ICacheService to interact with the cache 
    public sealed class CachingBehavior<TRequest, TResponse> 
        : IPipelineBehavior<TRequest, TResponse> 
        where TRequest : notnull
    {

        private readonly ICacheService _cache;

        public CachingBehavior(ICacheService cashe)
        {
            _cache = cashe;
        }

        // Implement the Handle method to add caching logic
        // for requests that implement ICacheableQuery
        // If the request is cacheable, check the cache for a response
        // If found, return the cached response
        // If not found, proceed to the next handler, cache the response, and return it 
        public async Task<TResponse> Handle(
            TRequest request,
            RequestHandlerDelegate<TResponse> next,
            CancellationToken cancellationToken)
        {
            // 1) Check if the request is cacheable
            // If not, proceed to the next handler 
            if (request is not ICacheableQuery cacheable)
                return await next();

            // 2) Try to get the response from the cache
            if (_cache.TryGetValue(cacheable.CacheKey, out TResponse? cached) && cached is not null)
                return cached;

            // 3) If not found in cache, proceed to the next handler
            var response = await next();

            // 4) Cache the response for future requests
            _cache.Set(cacheable.CacheKey, response, cacheable.SlidingExpiration);

            // 5) Return the response 
            return response;

        }
    }
}
