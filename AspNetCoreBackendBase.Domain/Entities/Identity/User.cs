using Microsoft.AspNetCore.Identity;

namespace AspNetCoreBackendBase.Domain.Entities
{
    /// <summary>
    /// Represents a user in the application, extending <see cref="IdentityUser{Guid}"/> to include additional user-related information.
    /// </summary>
    public class User : IdentityUser<Guid>
    {
        /// <summary>
        /// Gets or sets the user's first name.
        /// </summary>
        /// <value>A <see langword="string"/> representing the user's first name.</value>
        public required string Name { get; set; }

        /// <summary>
        /// Gets or sets the user's last name.
        /// </summary>
        /// <value>A <see langword="string"/> representing the user's last name.</value>
        public required string Surname { get; set; }

        /// <summary>
        /// Gets or sets the refresh token associated with the user.
        /// </summary>
        /// <value>A <see langword="string"/> representing the refresh token.</value>
        public string? RefreshToken { get; set; }

        /// <summary>
        /// Gets or sets the expiration date and time of the refresh token.
        /// </summary>
        /// <value>A <see cref="DateTime"/> representing the expiration date and time of the refresh token.</value>
        public DateTime? RefreshTokenExpiration { get; set; }
    }
}
