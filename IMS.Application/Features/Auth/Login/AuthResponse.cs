
namespace IMS.Application.Features.Auth.Login
{
    // This record represents the data structure for an authentication response,
    // containing the access token, its expiration time, user information, and roles.
    public sealed record AuthResponse(
        string AccessToken,
        DateTime ExpiresAtUtc,
        int UserId,
        string Email,
        IReadOnlyCollection<string> Roles);
}
