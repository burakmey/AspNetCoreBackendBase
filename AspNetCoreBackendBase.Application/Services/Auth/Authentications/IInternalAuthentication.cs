using AspNetCoreBackendBase.Application.CQRS.Commands.AuthCommands;
using AspNetCoreBackendBase.Application.DTOs;

namespace AspNetCoreBackendBase.Application.Services
{
    /// <summary>
    /// Defines methods for internal authentication services.
    /// </summary>
    public interface IInternalAuthentication
    {
        /// <summary>
        /// Asynchronously logs a user in with internal authentication.
        /// </summary>
        /// <param name="model">The <see cref="LoginCommandRequest"/> object containing the necessary information for internal login.</param>
        /// <returns>
        /// The <see cref="Task"/> that represents the asynchronous operation, containing a <see cref="BaseResponse{LoginCommandResponse}"/>
        /// indicating the success or failure of the login attempt and containing relevant response data.
        /// </returns>
        Task<BaseResponse<LoginCommandResponse>> LoginAsync(LoginCommandRequest model);

        /// <summary>
        /// Asynchronously refreshes an authentication token for a logged-in user.
        /// </summary>
        /// <param name="model">The <see cref="RefreshCommandRequest"/> object containing the necessary information to refresh the token.</param>
        /// <returns>
        /// The <see cref="Task"/> that represents the asynchronous operation, containing a <see cref="BaseResponse{RefreshCommandResponse}"/>
        /// indicating the success or failure of the token refresh attempt and containing the new token data if successful.
        /// </returns>
        Task<BaseResponse<RefreshCommandResponse>> RefreshAsync(RefreshCommandRequest model);
    }
}
