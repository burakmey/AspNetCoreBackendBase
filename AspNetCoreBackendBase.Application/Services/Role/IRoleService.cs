using AspNetCoreBackendBase.Application.CQRS.Commands.RoleCommands;
using AspNetCoreBackendBase.Application.CQRS.Queries.RoleQueries;
using AspNetCoreBackendBase.Application.DTOs;

namespace AspNetCoreBackendBase.Application.Services
{
    /// <summary>
    /// Defines methods for managing role services.
    /// </summary>
    public interface IRoleService
    {
        /// <summary>
        /// Asynchronously creates a new role with a unique name.
        /// </summary>
        /// <param name="model">The <see cref="CreateRoleCommandRequest"/> object containing the necessary information for creating the role.</param>
        /// <returns>
        /// The <see cref="Task"/> that represents the asynchronous operation, containing a <see cref="BaseResponse{object}"/>
        /// which indicates the success or failure of the role creation attempt. The response data object is generally <see langword="null"/>.
        /// </returns>
        Task<BaseResponse<object>> CreateRoleAsync(CreateRoleCommandRequest model);

        /// <summary>
        /// Asynchronously deletes a role.
        /// </summary>
        /// <param name="model">The <see cref="DeleteRoleCommandRequest"/> object containing the necessary information for deleting the role.</param>
        /// <returns>
        /// The <see cref="Task"/> that represents the asynchronous operation, containing a <see cref="BaseResponse{object}"/>
        /// which indicates the success or failure of the role deletion attempt. The response data object is generally <see langword="null"/>.
        /// </returns>
        Task<BaseResponse<object>> DeleteRoleAsync(DeleteRoleCommandRequest model);

        /// <summary>
        /// Asynchronously retrieves a specific role or a list of roles based on the query request.
        /// </summary>
        /// <param name="model">The <see cref="GetRolesQueryRequest"/> object containing the necessary information for retrieving roles.</param>
        /// <returns>
        /// A <see cref="BaseResponse{GetRolesQueryResponse}"/> containing the retrieved roles based on the query request.
        /// </returns>
        BaseResponse<GetRolesQueryResponse> GetRoles(GetRolesQueryRequest model);

        /// <summary>
        /// Asynchronously updates an existing role.
        /// </summary>
        /// <param name="model">The <see cref="UpdateRoleCommandRequest"/> object containing the necessary information for updating the role.</param>
        /// <returns>
        /// The <see cref="Task"/> that represents the asynchronous operation, containing a <see cref="BaseResponse{object}"/>
        /// which indicates the success or failure of the role update attempt. The response data object is generally <see langword="null"/>.
        /// </returns>
        Task<BaseResponse<object>> UpdateRoleAsync(UpdateRoleCommandRequest model);
    }
}
