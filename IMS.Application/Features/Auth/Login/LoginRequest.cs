
namespace IMS.Application.Features.Auth.Login
{
    // This record represents the data structure for a login request, containing the user's email and password.
    public sealed record LoginRequest(string Email, string Password);
}
