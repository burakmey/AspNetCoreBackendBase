using AspNetCoreBackendBase.Application.DTOs.Auth;
using AspNetCoreBackendBase.Domain.Entities;

namespace AspNetCoreBackendBase.Application.Services
{
    /// <summary>
    /// Defines methods for creating and managing tokens.
    /// </summary>
    public interface ITokenService
    {
        /// <summary>
        /// Creates an access token for a given user.
        /// </summary>
        /// <param name="user">
        /// The <see cref="User"/> for whom the access token is created. This user is typically authenticated and has valid credentials.
        /// </param>
        /// <returns>
        /// A <see cref="Token"/> object that contains the generated access token and refresh token.
        /// The access token is used to authenticate the user, while the refresh token is used to obtain a new access token when the original one expires.
        /// </returns>
        Token CreateAccessToken(User user);

        /// <summary>
        /// Creates a new refresh token.
        /// </summary>
        /// <returns>
        /// A <see langword="string"/> representing the newly created refresh token.
        /// The refresh token is used to obtain a new access token when the current access token expires.
        /// </returns>
        string CreateRefreshToken();
    }
}
