
using IMS.Application.Abstractions.Caching;
using IMS.Application.Common.Results;
using MediatR;
using Microsoft.Extensions.Logging;

namespace IMS.Application.Common.Behaviors
{
    // Pipeline behavior to handle cache version invalidation for requests
    // This behavior intercepts requests that implement IInvalidatesCachePrefix
    // and increments the version for specified cache prefixes
    public sealed class CacheVersionInvalidationBehavior<TRequest, TResponse>
        : IPipelineBehavior<TRequest, TResponse>
        where TRequest : notnull
    {

        private readonly ICacheVersionService _versions;

        private readonly ILogger<CacheVersionInvalidationBehavior<TRequest, TResponse>> _logger;


        public CacheVersionInvalidationBehavior(
            ICacheVersionService versions,
            ILogger<CacheVersionInvalidationBehavior<TRequest, TResponse>> logger)
        {
            _versions = versions;
            _logger = logger;
        }

        // Invalidate cache versions after the request is handled
        // This method is called after the request has been processed
        // and before the response is returned to the caller
        // It checks if the request invalidates cache prefixes
        // and if the response indicates success
        // If both conditions are met, it increments the versions
        // for the specified cache prefixes
        // Finally, it returns the original response
        // Parameters:
        // - request: The incoming request being processed
        // - next: Delegate to invoke the next handler in the pipeline
        // - cancellationToken: Token to monitor for cancellation requests
        // Returns:
        // - The response from the request handler
        public async Task<TResponse> Handle(
            TRequest request,
            RequestHandlerDelegate<TResponse> next,
            CancellationToken cancellationToken)
        {
            // 1) Proceed with the request
            var response = await next();

            // 2) Check if the request invalidates cache prefixes
            // If not, return the response as is 
            // without any cache version updates
            if (request is not IInvalidatesCachePrefix invalidator)
                return response;

            // 3) Check if the response indicates success
            // If not, skip cache invalidation and return the response
            if (response is not IResult result || !result.IsSuccess)
            {
                // Log that invalidation is skipped due to unsuccessful response
                _logger.LogInformation("[INVALIDATE] Skipped (request={Request} not success)", typeof(TRequest).Name);
                return response;
            }
                

            // 4) Invalidate the specified cache prefixes
            // This is done by incrementing their versions
            // which effectively marks cached entries as stale
            foreach (var prefix in invalidator.CachePrefixesToInvalidate)
            {
                // Increment the version for each cache prefix
                _versions.Increment(prefix);

                // Log the invalidation action
                _logger.LogInformation("[INVALIDATE] Incremented cache version for prefix: {Prefix}", prefix);
            }

            // 5) Return the original response
            return response;
        }
    }
}
