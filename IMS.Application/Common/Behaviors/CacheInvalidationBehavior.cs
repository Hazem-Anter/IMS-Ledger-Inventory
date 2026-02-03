
using IMS.Application.Abstractions.Caching;
using MediatR;

namespace IMS.Application.Common.Behaviors
{
    // Pipeline behavior to handle cache invalidation for commands
    // This behavior intercepts commands that implement ICacheInvalidatorCommand
    // and invalidates the specified cache entries after the command is handled
    public sealed class CacheInvalidationBehavior<TRequest, TResponse>
        : IPipelineBehavior<TRequest, TResponse>
        where TRequest : notnull
    {

        private readonly ICacheService _cache;
        public CacheInvalidationBehavior(ICacheService cache)
        {
            _cache = cache;
        }

        // Invalidate cache entries after the command is handled
        // This ensures that the cache is only invalidated if the command succeeds
        // and the data is actually changed
        public async Task<TResponse> Handle(
            TRequest request,
            RequestHandlerDelegate<TResponse> next,
            CancellationToken cancellationToken)
        {
            // 1) Process the request first 
            var response = await next();

            // 2) Check if the request is a cache invalidation command
            //   If not, simply return the response
            if (request is not ICacheInvalidatorCommand invalidator)
                return response;

            // 3) Invalidate each cache key specified by the command
            //  This is done after the command is handled to ensure that
            //  the cache is only invalidated if the command succeeds
            foreach (var key in invalidator.CacheKeysToInvalidate)
                _cache.Remove(key);

            // 4) Return the original response
            return response;
        }
    }
}
