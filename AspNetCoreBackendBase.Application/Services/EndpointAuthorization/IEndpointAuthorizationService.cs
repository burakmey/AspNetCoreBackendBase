using AspNetCoreBackendBase.Application.CQRS.Commands.EndpointAuthorizationCommands;
using AspNetCoreBackendBase.Application.CQRS.Queries.EndpointAuthorizationQueries;
using AspNetCoreBackendBase.Application.DTOs;

namespace AspNetCoreBackendBase.Application.Services
{
    /// <summary>
    /// Defines methods for managing endpoint authorization.
    /// </summary>
    public interface IEndpointAuthorizationService
    {
        /// <summary>
        /// Asynchronously assigns roles to a specified endpoint.
        /// </summary>
        /// <param name="model">The <see cref="AssignRolesToEndpointCommandRequest"/> object containing the necessary information for assigning roles.</param>
        /// <returns>
        /// The <see cref="Task"/> that represents the asynchronous operation, containing a <see cref="BaseResponse{object}"/>
        /// which indicates the success or failure of the role assignment attempt. The response data object is generally <see langword="null"/>.
        /// </returns>
        Task<BaseResponse<object>> AssignRolesToEndpointAsync(AssignRolesToEndpointCommandRequest model);

        /// <summary>
        /// Asynchronously gets the list of roles assigned to a specified endpoint.
        /// </summary>
        /// <param name="model">The <see cref="GetRolesForEndpointQueryRequest"/> object containing the necessary information to retrieve the roles.</param>
        /// <returns>
        /// The <see cref="Task"/> that represents the asynchronous operation, containing a <see cref="BaseResponse{GetRolesForEndpointQueryResponse}"/>
        /// which includes the list of roles assigned to the specified endpoint.
        /// </returns>
        Task<BaseResponse<GetRolesForEndpointQueryResponse>> GetRolesForEndpointAsync(GetRolesForEndpointQueryRequest model);
    }
}
