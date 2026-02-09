
using IMS.Application.Abstractions.Auth;
using Microsoft.AspNetCore.Http;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace IMS.Infrastructure.Identity
{
    // This service provides information about the currently authenticated user by accessing the HTTP context.
    // It implements the ICurrentUserService interface, which defines the contract for retrieving user information.
    public sealed class CurrentUserService : ICurrentUserService
    {
        // The IHttpContextAccessor is used to access the current HTTP context,
        // which contains information about the authenticated user.
        // like the user claims, authentication status, etc.
        private readonly IHttpContextAccessor _http;

        public CurrentUserService(IHttpContextAccessor http)
        {
            _http = http;
        }

        // The userId property retrieves the user ID from the JWT token claims.
        public int? UserId
        {
            get 
            {
                // 1) Access the current HTTP context and check if the user is authenticated.
                // If the user is not authenticated, return null.
                var user = _http.HttpContext?.User;
                if(user is null || user.Identity?.IsAuthenticated != true)
                    return null;

                // 2) Extract the user ID from the JWT token claims.
                var sub = user.FindFirstValue(JwtRegisteredClaimNames.Sub)
                    ?? user.FindFirstValue(ClaimTypes.NameIdentifier);

                // 3) Validate the extracted user ID (e.g., check if it's a valid integer).
                return int.TryParse(sub, out var id) ? id : null;
            }
        }

        // The IsAuthenticated property checks if the user is authenticated by accessing the HTTP context.
        public bool IsAuthenticated 
            => _http.HttpContext?.User?.Identity?.IsAuthenticated == true;

        public string? DisplayName
        {
            get
            {
                // 1) Access the current HTTP context and check if the user is authenticated.
                // If the user is not authenticated, return null.
                var user = _http.HttpContext?.User;
                if (user is null || user.Identity?.IsAuthenticated != true)
                    return null;

                 
                return user.FindFirstValue(JwtRegisteredClaimNames.Email)
                            ?? user.FindFirstValue(ClaimTypes.Email)
                            ?? user.Identity?.Name;
            }
        }
    }
}
