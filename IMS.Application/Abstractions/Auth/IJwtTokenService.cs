
namespace IMS.Application.Abstractions.Auth
{
    // IJwtTokenService defines a contract for a service that creates JWT tokens for authentication purposes.
    // The CreateToken method takes a user ID, email, and a collection of roles,
    // and returns a JwtTokenResult containing the access token and its expiration time.
    public interface IJwtTokenService
    {
        JwtTokenResult CreateToken(int userId, string email, IReadOnlyCollection<string> roles);
    }

    
    public sealed record JwtTokenResult(string AccessToken, DateTime ExpiresAtUtc);
}
