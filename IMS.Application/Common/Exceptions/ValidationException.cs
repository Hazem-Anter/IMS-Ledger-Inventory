
namespace IMS.Application.Common.Exceptions
{
    // The ValidationException is used to indicate that one or more validation errors occurred during the processing of a request.
    // For example, this exception can be thrown when the input data for a request fails to meet certain validation criteria (e.g., required fields are missing, values are out of range, etc.).
    // This exception should be caught by the global exception handling middleware (GlobalExceptionMiddleware) to return a standardized error response with a 400 Bad Request status code,
    public sealed class ValidationException : Exception
    {
        // The Errors property is a dictionary that contains the validation errors,
        // where the key is the name of the field that failed validation and the value is an array of error messages associated with that field.
        // This structure allows for multiple validation errors to be associated with a single field,
        // providing detailed feedback to the client about what went wrong with their request.
        public IReadOnlyDictionary<string, string[]>? Errors { get; }
        public ValidationException(IReadOnlyDictionary<string, string[]>? errors)
            : base("One or more validation errors occurred.")
        {
            Errors = errors;
        }
    }
}
