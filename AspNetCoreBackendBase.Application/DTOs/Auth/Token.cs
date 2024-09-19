namespace AspNetCoreBackendBase.Application.DTOs.Auth
{
    /// <summary>
    /// Represents a token used for authentication purposes, including access and refresh tokens.
    /// </summary>
    public class Token
    {
        /// <summary>
        /// Gets or sets the access token issued for authentication.
        /// </summary>
        /// <value>A <see langword="string"/> representing the access token.</value>
        public required string AccessToken { get; set; }

        /// <summary>
        /// Gets or sets the expiration date and time of the access token.
        /// </summary>
        /// <value>A <see cref="DateTime"/> indicating when the access token expires.</value>
        public required DateTime AccessTokenExpiration { get; set; }

        /// <summary>
        /// Gets or sets the refresh token used to obtain a new access token.
        /// </summary>
        /// <value>A <see langword="string"/> representing the refresh token.</value>
        public required string RefreshToken { get; set; }
    }
}
