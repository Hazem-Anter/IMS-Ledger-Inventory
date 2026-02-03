
using IMS.Application.Abstractions.Caching;
using IMS.Application.Common.Results;
using MediatR;

namespace IMS.Application.Common.Behaviors
{
    // Pipeline behavior to handle cache version invalidation for requests
    // This behavior intercepts requests that implement IInvalidatesCachePrefix
    // and increments the version for specified cache prefixes
    public sealed class CacheVersionInvalidationBehavior<IRequest, TResponse>
        : IPipelineBehavior<IRequest, TResponse>
        where IRequest : notnull
    {

        private readonly ICacheVersionService _versions;

        public CacheVersionInvalidationBehavior(ICacheVersionService versions)
        {
            _versions = versions;
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
            IRequest request,
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
            // If not, return the response as is
            // without any cache version updates
            if (response is not IResult result || !result.IsSuccess)
                return response;

            // 4) Invalidate the specified cache prefixes
            // This is done by incrementing their versions
            // which effectively marks cached entries as stale
            foreach (var prefix in invalidator.CachePrefixesToInvalidate)
            {
                // Increment the version for each cache prefix
                _versions.Increment(prefix);
            }

            // 5) Return the original response
            return response;
        }
    }
}
