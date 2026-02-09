
using IMS.Application.Common.Results;
using MediatR;

namespace IMS.Application.Features.Auth.Users.CreateUser
{
    // Command to create a new user with specified email, password, and roles.
    public sealed record CreateUserCommand(
        string Email,
        string Password,
        IReadOnlyList<string> Roles

    ) : IRequest<Result<CreateUserResponse>>;
}
