using AspNetCoreBackendBase.Application;
using AspNetCoreBackendBase.Application.DTOs.Auth;
using AspNetCoreBackendBase.Application.Services;
using AspNetCoreBackendBase.Domain.Entities;
using AspNetCoreBackendBase.Infrastructure.Options;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace AspNetCoreBackendBase.Infrastructure.Services
{
    public class TokenService : ITokenService
    {
        readonly TokenOptions _tokenOptions;

        public TokenService(IOptions<TokenOptions> tokenOptions)
        {
            _tokenOptions = tokenOptions.Value;
        }

        public Token CreateAccessToken(User user)
        {
            // Create a symmetric security key from the configuration.
            SymmetricSecurityKey securityKey = new(Encoding.UTF8.GetBytes(_tokenOptions.SecurityKey));

            // Define signing credentials using the security key and HMAC SHA-256 algorithm.
            SigningCredentials signingCredentials = new(securityKey, SecurityAlgorithms.HmacSha256);

            // Get the access token expiration time from the configuration.
            int expirationMinute = int.Parse(Configuration.GetTokenAccessTokenMinute);

            // Create the JWT security token with the necessary claims and settings.
            JwtSecurityToken securityToken = new(
                audience: _tokenOptions.Audience,
                issuer: _tokenOptions.Issuer,
                claims: new List<Claim>
                {
                    new(ClaimTypes.Name, user.UserName),
                    new(ClaimTypes.Email, user.Email),
                },
                expires: DateTime.UtcNow.AddMinutes(expirationMinute),
                notBefore: DateTime.UtcNow,
                signingCredentials: signingCredentials
            );

            // Instantiate a JWT security token handler to serialize the token.
            JwtSecurityTokenHandler securityTokenHandler = new();

            // Create the Token object with the serialized access token and a generated refresh token.
            Token token = new()
            {
                AccessToken = securityTokenHandler.WriteToken(securityToken),
                AccessTokenExpiration = DateTime.UtcNow.AddMinutes(expirationMinute),
                RefreshToken = CreateRefreshToken(),
            };

            return token;
        }

        public string CreateRefreshToken()
        {
            // Generate a random byte array for the refresh token.
            byte[] numbers = new byte[32];
            using RandomNumberGenerator random = RandomNumberGenerator.Create();
            random.GetBytes(numbers);

            // Convert the byte array to a Base64 string.
            return Convert.ToBase64String(numbers);
        }
    }
}
