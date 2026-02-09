
using IMS.Application.Common.Results;
using MediatR;

namespace IMS.Application.Features.Auth.Login
{
    // This record represents the command for user login, containing the user's email and password.
    public sealed record LoginCommand(
        string Email,
        string Password

        ) : IRequest<Result<AuthResponse>>;
}
