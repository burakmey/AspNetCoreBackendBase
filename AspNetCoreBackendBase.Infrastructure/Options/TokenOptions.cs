using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Text;

namespace AspNetCoreBackendBase.Infrastructure.Options
{
    /// <summary>
    /// Represents the configuration options for generating and validating JWT tokens.
    /// </summary>
    public class TokenOptions
    {
        /// <summary>
        /// The configuration key used to retrieve the token options from the configuration file.
        /// </summary>
        public const string Options = "Token:Options";

        /// <summary>
        /// Gets or sets the audience for the JWT tokens.
        /// </summary>
        /// <value>
        /// A <see langword="string"/> representing the expected audience of the JWT tokens.
        /// </value>
        public required string Audience { get; init; }

        /// <summary>
        /// Gets or sets the issuer for the JWT tokens.
        /// </summary>
        /// <value>
        /// A <see langword="string"/> representing the expected issuer of the JWT tokens.
        /// </value>
        public required string Issuer { get; init; }

        /// <summary>
        /// Gets or sets the security key used to sign the JWT tokens.
        /// </summary>
        /// <value>
        /// A <see langword="string"/> representing the security key used for signing the tokens. This key should be kept secret and secure.
        /// </value>
        public required string SecurityKey { get; init; }

        /// <summary>
        /// Gets the token validation parameters used to validate the JWT tokens.
        /// </summary>
        /// <value>
        /// A <see cref="Microsoft.IdentityModel.Tokens.TokenValidationParameters"/> object that contains the rules for validating JWT tokens.
        /// </value>
        public TokenValidationParameters TokenValidationParameters => new()
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(SecurityKey)),
            ValidateIssuer = true,
            ValidIssuer = Issuer,
            ValidateAudience = true,
            ValidAudience = Audience,
            ValidateLifetime = true,
            LifetimeValidator = (notBefore, expires, securityToken, validationParameters) => expires != null && expires > DateTime.UtcNow,
            NameClaimType = ClaimTypes.Name
        };
    }
}
