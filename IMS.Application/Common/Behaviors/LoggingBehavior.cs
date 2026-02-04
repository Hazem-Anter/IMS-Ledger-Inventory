
using IMS.Application.Common.Results;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Diagnostics;

namespace IMS.Application.Common.Behaviors
{
    // Pipeline behavior to log request handling details
    // This behavior logs when a request starts handling,
    // when it finishes, the time taken, and any errors encountered
    // It helps in monitoring and diagnosing issues in request processing
    public sealed class LoggingBehavior<TRequest, TResponse>
        : IPipelineBehavior<TRequest, TResponse>
        where TRequest : notnull
    {
        // Logger instance for logging request handling information
        private readonly ILogger<LoggingBehavior<TRequest, TResponse>> _logger;

        public LoggingBehavior(ILogger<LoggingBehavior<TRequest, TResponse>> logger)
        {
            _logger = logger;
        }

        // Handle method to log request processing details
        // It logs the start and end of request handling,
        // the time taken, and any errors if they occur
        // It uses a Stopwatch to measure elapsed time
        // It also checks if the response implements IResult to log success or failure
        // Parameters:
        // - request: The incoming request to be handled
        // - next: The next delegate in the pipeline to call
        // - cancellationToken: Token to monitor for cancellation requests
        // Returns: The response from the request handling
        public async Task<TResponse> Handle(
            TRequest request,
            RequestHandlerDelegate<TResponse> next,
            CancellationToken cancellationToken)
        {
            // 1) Get the name of the request type for logging
            // ex: "GetStockOverviewQuery" for GetStockOverviewQuery request
            var requestName = typeof(TRequest).Name;

            // 2) Log the start of request handling 
            // ex: "Handling GetStockOverviewQuery" 
            // before calling the next delegate in the pipeline to process the request
            _logger.LogInformation("Handling {RequestName}", requestName);

            // 3) Start a stopwatch to measure elapsed time for handling the request
            var sw = Stopwatch.StartNew();
            try
            {
                // 4) Call the next delegate in the pipeline to process the request
                // This will invoke the actual request handler and get the response
                var response = await next();

                // 5) Stop the stopwatch as request handling is complete
                sw.Stop();

                // 6) Log the completion of request handling with elapsed time
                // If the response implements IResult, log success or failure details
                if (response is IResult r)
                {
                    // if success, log as Information; if failure, log as Warning with error details
                    if (r.IsSuccess)
                        _logger.LogInformation("Handled {RequestName} SUCCESS in {ElapsedMs}ms", requestName, sw.ElapsedMilliseconds);
                    else
                        _logger.LogWarning("Handled {RequestName} FAIL in {ElapsedMs}ms. Error: {Error}", requestName, sw.ElapsedMilliseconds, r.Error);
                }
                else
                {
                    // If response does not implement IResult, just log completion with elapsed time
                    _logger.LogInformation("Handled {RequestName} in {ElapsedMs}ms", requestName, sw.ElapsedMilliseconds);
                }

                // 7) Return the response from the request handling to the caller
                // This could be a successful result or an error result 
                return response;
            }
            catch (Exception ex)
            {
                // 8) In case of exception during request handling, stop the stopwatch
                // Log the error with exception details and elapsed time
                sw.Stop();
                _logger.LogError(ex, "Unhandled exception in {RequestName} after {ElapsedMs}ms", requestName, sw.ElapsedMilliseconds);
                throw;
            }
        }
    }
}
