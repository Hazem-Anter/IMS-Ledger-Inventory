
namespace IMS.Application.Common.Exceptions
{
    // The NotFoundException is used to indicate that a requested resource was not found.
    // For example, this exception can be thrown when trying to retrieve a resource by its ID that does not exist in the database.
    // This exception should be caught by the global exception handling middleware (GlobalExceptionMiddleware) to return a standardized error response with a 404 Not Found status code.
    public sealed class NotFoundException : Exception
    {
        // The constructor takes a message parameter that describes the nature of the not found error and passes it to the base Exception class.
        public NotFoundException(string message) : base(message) { }
    }
}
