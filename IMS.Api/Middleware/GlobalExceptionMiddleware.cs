
using IMS.Application.Common.Exceptions;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace IMS.Api.Middleware
{
    // Global exception handling middleware to catch unhandled exceptions and return standardized error responses
    // This middleware should be registered early in the pipeline to ensure it can catch exceptions from all downstream components
    // It maps specific exception types to appropriate HTTP status codes and error messages,
    // and can be extended to include more exception types as needed
    public sealed class GlobalExceptionMiddleware : IMiddleware
    {
        // The IHostEnvironment is injected to determine the current hosting environment (e.g., Development, Production).
        // This allows the middleware to include detailed exception information in the response when running in a development environment,
        // while providing a generic error message in production to avoid exposing sensitive information.
        private readonly IHostEnvironment _env;

        public GlobalExceptionMiddleware(IHostEnvironment env)
        {
            _env = env;
        }

        // The InvokeAsync method is the core of the middleware,
        // where it wraps the execution of the next middleware in a try-catch block.
        // If an exception is thrown by any downstream middleware or the request handling logic,
        // it is caught and passed to the WriteProblemDetailsAsync method,
        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            try
            {
                // Invoke the next middleware in the pipeline.
                // If any unhandled exceptions occur,
                // they will be caught by the catch block below.
                await next(context);
            }
            catch (Exception ex)
            {
                await WriteProblemDetailsAsync(context, ex);
            }
        }

        // The WriteProblemDetailsAsync method constructs a ProblemDetails object based on the type of exception caught.
        // It maps specific exception types to corresponding HTTP status codes and error titles,
        // and includes additional details such as the exception message (in development) and a trace identifier
        private async Task WriteProblemDetailsAsync(HttpContext context, Exception ex)
        {
            // 1) Map specific exception types to corresponding HTTP status codes and error titles.
            var (status, title) = ex switch
            {
                NotFoundException => (StatusCodes.Status404NotFound, "Not Found"),
                ConflictException => (StatusCodes.Status409Conflict, "Conflict"),
                ValidationException => (StatusCodes.Status400BadRequest, "Validation Error"),
                UnauthorizedAccessException => (StatusCodes.Status401Unauthorized, "Unauthorized"),
                _ => (StatusCodes.Status500InternalServerError, "Internal Server Error")
            };

            // 2) Construct a ProblemDetails object to standardize the error response format.
            // The ProblemDetails class is part of the ASP.NET Core framework and provides a standardized way to represent errors in HTTP responses.
            // It includes properties such as Status, Title, Detail, Type, and Instance, which help clients understand the nature of the error and how to handle it.

            // The Detail property: includes the exception message when running in a development environment, and a generic error message in production to avoid exposing sensitive information.
            // The Type property: provides a URI reference to the error type, which can be used by clients to understand the error and potentially provide more specific handling based on the error type.
            // The Instance property: includes the request path, which can help with debugging and tracing the error in logs.
            var problem = new ProblemDetails
            {
                Status = status,
                Title = title,
                Detail = _env.IsDevelopment() ? ex.ToString() : "An error occurred.",
                Type = $"https://httpstatuses.com/{status}",
                Instance = context.Request.Path
            };

            // 3) Include additional details such as the exception message (in development) and a trace identifier to help with debugging and tracing the error in logs.
            // The trace identifier is a unique identifier for the request, which can be used to correlate logs and trace the flow of the request through the system.
            // If the exception is a ValidationException, it also includes the validation errors in the response, which can help clients understand what went wrong with their request and how to fix it.
            problem.Extensions["traceId"] = context.TraceIdentifier;

            if (ex is ValidationException vex)
                problem.Extensions["errors"] = vex.Errors;

            // 4) Set the response status code and content type, and write the serialized ProblemDetails object to the response body.
            // The content type is set to "application/problem+json" to indicate that the response body contains a ProblemDetails object in JSON format.
            // The ProblemDetails object is serialized to JSON using the System.Text.Json serializer, which is the default JSON serializer in ASP.NET Core.
            context.Response.StatusCode = status;
            context.Response.ContentType = "application/problem+json";

            // Write the serialized ProblemDetails object to the response body.
            // This allows clients to receive a standardized error response that includes all the relevant information about the error,
            // such as the status code, title, detail, type, instance, and any additional extensions.
            await context.Response.WriteAsync(JsonSerializer.Serialize(problem));
        }
    }
}
