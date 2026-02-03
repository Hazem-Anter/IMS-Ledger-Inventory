
namespace IMS.Application.Common.Results
{
    public interface IResult
    {
        bool IsSuccess { get; }
        string? Error { get; }
    }
}
