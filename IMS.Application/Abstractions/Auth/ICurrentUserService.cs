
namespace IMS.Application.Abstractions.Auth
{
    // This interface defines the contract for a service that provides information about
    // the currently authenticated user. 
    public interface ICurrentUserService
    {
        bool IsAuthenticated { get; }
        int? UserId { get; }
        string? DisplayName { get; } // we’ll use Email for now
    }
}
