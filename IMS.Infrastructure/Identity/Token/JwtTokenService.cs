
using IMS.Application.Abstractions.Auth;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace IMS.Infrastructure.Identity.Token
{
    // This class is responsible for creating JWT tokens based on the provided user information and roles.
    // It uses the JwtOptions to configure the token generation,
    // including the secret key, issuer, audience, and expiry time.
    public sealed class JwtTokenService : IJwtTokenService
    {
        
        private readonly JwtOptions _options;

        public JwtTokenService(IOptions<JwtOptions> options)
        {
            _options = options.Value;
        }

        // The CreateToken method generates a JWT token for a given user ID, email, and roles.
        public JwtTokenResult CreateToken(
            int userId,
            string email,
            string securityStamp,
            IReadOnlyCollection<string> roles)
        {
            // 1) Set the current time and calculate the token's expiration time based on the configured expiry minutes.
            var now = DateTime.UtcNow;
            var expires = now.AddMinutes(_options.ExpiryMinutes);

            // 2) Create a list of claims that will be included in the token.
            // This includes the user's ID, email, and a unique identifier (JTI).
            // (JTI) : is a unique identifier for the token,
            // which can be used to prevent token replay attacks.
            // Example: if a token is stolen, the JTI can be used to identify and invalidate that specific token.

            // The "sstamp" claim is added to include the user's security stamp,
            // which can be used to invalidate tokens if the user's security information changes (e.g., password reset).
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, userId.ToString()),
                new Claim(JwtRegisteredClaimNames.Email, email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim("sstamp", securityStamp)
            };
            // Add claims for each role the user has. This allows the token to carry information about the user's permissions and access levels.
            foreach (var role in roles)
                claims.Add(new Claim(ClaimTypes.Role, role));

            // 3) Create a symmetric security key using the secret from the options,
            // and then create signing credentials using that key and the HMAC SHA256 algorithm.
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_options.Secret));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            // 4) Create a JwtSecurityToken using the issuer, audience, claims, notBefore, expires, and signing credentials.
            var token = new JwtSecurityToken(
                issuer: _options.Issuer,
                audience: _options.Audience,
                claims: claims,
                notBefore: now,
                expires: expires,
                signingCredentials: creds);

            // 5) Write the token to a string format using the JwtSecurityTokenHandler.
            var accessToken = new JwtSecurityTokenHandler().WriteToken(token);

            // 6) Return a JwtTokenResult containing the generated access token and its expiration time.
            return new JwtTokenResult(accessToken, expires);

        }
    }
}
