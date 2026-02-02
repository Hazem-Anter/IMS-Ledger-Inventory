
namespace IMS.Application.Common.Result
{
    // A generic result class to encapsulate success/failure states along with associated values or error messages.
    // T represents the type of the value returned on success.
    public sealed class Result<T>
    {
        public bool IsSuccess { get; }
        public string? Error { get; }
        public T? Value { get; }

        // Private constructor to enforce the use of static factory methods.
        private Result(bool isSuccess, T? value, string? error)
        {
            IsSuccess = isSuccess;
            Value = value;
            Error = error;
        }

        // factory methods are designed to create instances of Result<T> for success and failure scenarios.
        public static Result<T> Ok(T value) => new Result<T>(true, value, null);
        public static Result<T> Fail(string error) => new Result<T>(false, default, error);
    }
}
