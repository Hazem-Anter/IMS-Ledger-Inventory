
namespace IMS.Application.Common.Exceptions
{
    // The ConflictException is used to indicate that a request could not be processed because of a conflict with the current state of the resource.
    // For example, this exception can be thrown when trying to create a resource that already exists or when there is a version conflict during an update operation.
    // This exception should be caught by the global exception handling middleware (GlobalExceptionMiddleware) to return a standardized error response with a 409 Conflict status code.
    public sealed class ConflictException : Exception
    {
        // The constructor takes a message parameter that describes the nature of the conflict and passes it to the base Exception class.
        public ConflictException(string message) : base(message) { }
    }
}
