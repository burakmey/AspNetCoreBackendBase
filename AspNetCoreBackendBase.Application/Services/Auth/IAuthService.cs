using AspNetCoreBackendBase.Application.CQRS.Commands.AuthCommands;
using AspNetCoreBackendBase.Application.DTOs;
using AspNetCoreBackendBase.Domain.Entities;

namespace AspNetCoreBackendBase.Application.Services
{
    /// <summary>
    /// Defines methods for authentication services, including both external and internal authentication.
    /// </summary>
    public interface IAuthService : IExternalAuthentication, IInternalAuthentication
    {
        /// <summary>
        /// Asynchronously resets a user's password.
        /// </summary>
        /// <param name="model">The <see cref="ResetPasswordCommandRequest"/> object containing the necessary information for resetting the password.</param>
        /// <returns>
        /// The <see cref="Task"/> that represents the asynchronous operation, containing a <see cref="BaseResponse{object}"/>
        /// which indicates the success or failure of the password reset attempt. The response data object is generally <see langword="null"/>.
        /// </returns>
        Task<BaseResponse<object>> ResetPasswordAsync(ResetPasswordCommandRequest model);

        /// <summary>
        /// Asynchronously verifies a password reset token.
        /// </summary>
        /// <param name="model">The <see cref="VerifyResetTokenCommandRequest"/> object containing the token and other necessary data for verification.</param>
        /// <returns>
        /// The <see cref="Task"/> that represents the asynchronous operation, containing a <see cref="BaseResponse{object}"/>
        /// which indicates whether the token is valid and if the verification was successful. The response data object is generally <see langword="null"/>.
        /// </returns>
        Task<BaseResponse<object>> VerifyResetTokenAsync(VerifyResetTokenCommandRequest model);

        /// <summary>
        /// Asynchronously updates the refresh token for a user.
        /// </summary>
        /// <param name="refreshToken">The new refresh token to be updated.</param>
        /// <param name="user">The <see cref="User"/> for whom the refresh token is being updated.</param>
        /// <param name="accessTokenExpiration">The date and time when the access token will expire.</param>
        /// <returns>
        /// A <see cref="Task"/> representing the asynchronous operation.
        /// </returns>
        Task UpdateRefreshTokenAsync(string refreshToken, User user, DateTime accessTokenExpiration);
    }
}
