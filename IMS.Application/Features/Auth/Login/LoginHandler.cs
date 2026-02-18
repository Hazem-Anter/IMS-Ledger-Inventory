
using IMS.Application.Abstractions.Auth;
using IMS.Application.Common.Results;
using MediatR;

namespace IMS.Application.Features.Auth.Login
{
    // This class handles the login command by validating the user's credentials,
    // retrieving their roles, and generating a JWT token if the credentials are valid.
    public sealed class LoginHandler
        : IRequestHandler<LoginCommand, Result<AuthResponse>>
    {

        private readonly IAuthService _auth;
        private readonly IJwtTokenService _jwt;

        public LoginHandler(IAuthService auth, IJwtTokenService jwt)
        {
            _auth = auth;
            _jwt = jwt;
        }

        public async Task<Result<AuthResponse>> Handle(
            LoginCommand cmd,
            CancellationToken ct)
        {
            // 1) Validate the user's credentials using the authentication service.
            // If the credentials are invalid, return a failure result with an appropriate error message.
            var user = await _auth.ValidateUserAsync(cmd.Email, cmd.Password, ct);
            if (user is null)
                return Result<AuthResponse>.Fail("Invalid email or password");

            // 2) If the credentials are valid, retrieve the user's roles using the authentication service.
            var roles = await _auth.GetRolesAsync(user.UserId, ct);

            // 3) Generate a JWT token for the user using the JWT token service,
            // including the user's ID, email, security stamp, and roles.
            // The security stamp is used to ensure that the token is invalidated if the user's security information changes (e.g., password reset).
            var token = _jwt.CreateToken(user.UserId, user.Email, user.SecurityStamp, roles);

            // 4) Return a successful result containing an AuthResponse object with the generated token,
            return Result<AuthResponse>.Ok(
                new AuthResponse(token.AccessToken, token.ExpiresAtUtc, user.UserId, user.Email, roles));
        }
    }
}
