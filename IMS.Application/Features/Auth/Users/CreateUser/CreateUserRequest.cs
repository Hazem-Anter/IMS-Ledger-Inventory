
namespace IMS.Application.Features.Auth.Users.CreateUser
{
    // Request to create a new user with specified email, password, and roles.
    public sealed record CreateUserRequest(
        string Email,
        string Password,
        List<string> Roles
        );
}
