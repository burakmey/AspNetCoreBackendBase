using Application.Features.Commands.AuthCommands;
using Application.Features.Commands.UserCommands;
using AspNetCoreBackendBase.Application.CQRS.Commands.UserCommands;
using AspNetCoreBackendBase.Application.CQRS.Queries.UserQueries;
using AspNetCoreBackendBase.Application.DTOs;
using AspNetCoreBackendBase.Domain.Entities;

namespace AspNetCoreBackendBase.Application.Services
{
    /// <summary>
    /// Defines methods for managing user services.
    /// </summary>
    public interface IUserService
    {
        /// <summary>
        /// Asynchronously creates a new user.
        /// </summary>
        /// <param name="model">
        /// The <see cref="RegisterCommandRequest"/> object containing the necessary information for user registration, including user details and credentials.
        /// </param>
        /// <returns>
        /// The <see cref="Task"/> that represents the asynchronous operation, containing a <see cref="BaseResponse{object}"/> indicating the success or failure of the user creation attempt.
        /// </returns>
        Task<BaseResponse<object>> CreateUserAsync(RegisterCommandRequest model);

        /// <summary>
        /// Asynchronously updates a user's password.
        /// </summary>
        /// <param name="model">
        /// The <see cref="UpdatePasswordCommandRequest"/> object containing the necessary information for updating the password, including the current and new passwords.
        /// </param>
        /// <returns>
        /// The <see cref="Task"/> that represents the asynchronous operation, containing a <see cref="BaseResponse{object}"/> indicating the success or failure of the password update attempt.
        /// </returns>
        Task<BaseResponse<object>> UpdatePasswordAsync(UpdatePasswordCommandRequest model);

        /// <summary>
        /// Asynchronously gets the roles assigned to a user.
        /// </summary>
        /// <param name="model">
        /// The <see cref="GetUserRolesQueryRequest"/> object containing the necessary information to query user roles, such as the user identifier.
        /// </param>
        /// <returns>
        /// The <see cref="Task"/> that represents the asynchronous operation, containing a <see cref="BaseResponse{GetUserRolesQueryResponse}"/> with the roles assigned to the user.
        /// </returns>
        Task<BaseResponse<GetUserRolesQueryResponse>> GetUserRolesAsync(GetUserRolesQueryRequest model);

        /// <summary>
        /// Asynchronously assigns one or more roles to the user.
        /// </summary>
        /// <param name="model">
        /// The <see cref="AssignUserRolesCommandRequest"/> object containing the necessary information to assign roles, including the user identifier and the roles to be assigned.
        /// </param>
        /// <returns>
        /// The <see cref="Task"/> that represents the asynchronous operation, containing a <see cref="BaseResponse{object}"/> indicating the success or failure of the role assignment attempt.
        /// </returns>
        Task<BaseResponse<object>> AssignUserRolesAsync(AssignUserRolesCommandRequest model);

        /// <summary>
        /// Asynchronously gets all roles assigned for the user.
        /// </summary>
        /// <param name="userIdOrUserName">
        /// A <see langword="string"/> representing either the user identifier or username for which the roles are being retrieved.
        /// </param>
        /// <returns>
        /// The <see cref="Task"/> that represents the asynchronous operation, containing an array of <see langword="string"/> representing the roles assigned to the user.
        /// </returns>
        Task<string[]> GetAllRolesForUserAsync(string userIdOrUserName);

        /// <summary>
        /// Asynchronously gets the user by Id or UserName.
        /// </summary>
        /// <param name="userIdOrUserName">
        /// A <see langword="string"/> representing either the user identifier or username for which the user information is being retrieved.
        /// </param>
        /// <returns>
        /// The <see cref="Task"/> that represents the asynchronous operation, containing a <see cref="User"/> object representing the user with the specified Id or username.
        /// </returns>
        Task<User> GetUserAsync(string userIdOrUserName);

        /// <summary>
        /// Asynchronously checks if the user has the required role permissions for a specific endpoint.
        /// </summary>
        /// <param name="userName">
        /// A <see langword="string"/> representing the username of the user whose permissions are being checked.
        /// </param>
        /// <param name="code">
        /// A <see langword="string"/> representing the code for the endpoint to check against the user's roles.
        /// </param>
        /// <param name="route">
        /// A <see langword="string"/> representing the controller name (route) associated with the endpoint.
        /// </param>
        /// <returns>
        /// The <see cref="Task"/> that represents the asynchronous operation, containing a <see langword="bool"/> indicating whether the user has the required role permissions for the endpoint.
        /// </returns>
        Task<bool> HasRolePermissionForEndpointAsync(string userName, string code, string route);
    }
}
