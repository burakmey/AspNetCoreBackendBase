using AspNetCoreBackendBase.Application.CQRS.Commands.AuthCommands;
using AspNetCoreBackendBase.Application.DTOs;

namespace AspNetCoreBackendBase.Application.Services
{
    /// <summary>
    /// Defines methods for external authentication services.
    /// </summary>
    public interface IExternalAuthentication
    {
        /// <summary>
        /// Asynchronously logs a user in through Google authentication.
        /// </summary>
        /// <param name="model">The <see cref="LoginGoogleCommandRequest"/> object containing the necessary information for Google login.</param>
        /// <returns>
        /// The <see cref="Task"/> that represents the asynchronous operation, containing a <see cref="BaseResponse{LoginGoogleCommandResponse}"/>
        /// indicating the success or failure of the login attempt and containing relevant response data.
        /// </returns>
        Task<BaseResponse<LoginGoogleCommandResponse>> GoogleLoginAsync(LoginGoogleCommandRequest model);
    }
}
