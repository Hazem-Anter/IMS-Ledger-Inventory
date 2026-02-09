
namespace IMS.Application.Features.Auth.Users.CreateUser
{
    // Response containing the details of the newly created user, including their ID, email, and assigned roles.
    public sealed record CreateUserResponse(
        int UserId,
        string Email,
        IReadOnlyCollection<string> Roles
    );
}
