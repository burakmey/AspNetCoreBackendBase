using AspNetCoreBackendBase.Application.CQRS.Commands.RoleCommands;
using AspNetCoreBackendBase.Application.CQRS.Queries.RoleQueries;
using AspNetCoreBackendBase.Application.DTOs;
using AspNetCoreBackendBase.Application.Services;
using AspNetCoreBackendBase.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.CodeAnalysis;

namespace AspNetCoreBackendBase.Persistence.Services
{
    public class RoleService : IRoleService
    {
        readonly RoleManager<Role> _roleManager;

        public RoleService(RoleManager<Role> roleManager)
        {
            _roleManager = roleManager;
        }

        public async Task<BaseResponse<object>> CreateRoleAsync(CreateRoleCommandRequest model)
        {
            // Create a new Role entity using the provided role name.
            IdentityResult result = await _roleManager.CreateAsync(new Role(model.Name));

            // ROle 4 cant created but Role  4 created
            // Create a message based on the result of the operation.
            string message = result.Succeeded ? $"Role: {model.Name} created successfully." : string.Join("; ", result.Errors.Select(e => e.Description));

            // If the operation succeeded, return a success message. Otherwise, return error details.
            return new()
            {
                IsSuccessful = result.Succeeded,
                Message = message
            };
        }

        public async Task<BaseResponse<object>> DeleteRoleAsync(DeleteRoleCommandRequest model)
        {
            // Find the role by its identifier.
            Role? role = await _roleManager.FindByIdAsync(model.Id);
            //Role? role = await _roleManager.FindByNameAsync(model.Name);

            // If the role does not exist, throw exception.
            if (role == null)
                throw new Exception("Role not found.");

            // Attempt to delete the role and capture the result.
            IdentityResult result = await _roleManager.DeleteAsync(role);

            // Create a message based on the result of the operation.
            string message = result.Succeeded ? "Role deleted successfully." : string.Join("; ", result.Errors.Select(e => e.Description));

            // If the operation succeeded, return a success message. Otherwise, return error details.
            return new()
            {
                IsSuccessful = result.Succeeded,
                Message = message
            };
        }

        public BaseResponse<GetRolesQueryResponse> GetRoles(GetRolesQueryRequest model)
        {
            // Validate pagination parameters to ensure they are valid.
            if (model.Pagination.Page < 0 || model.Pagination.Size <= 0)
                throw new Exception("Invalid pagination parameters.");

            // Retrieve roles from the RoleManager.
            var roles = _roleManager.Roles
                .Select(r => r.Name!)
                .ToList();

            // Return a successful response containing the roles.
            return new()
            {
                IsSuccessful = true,
                Data = new() { Roles = roles }
            };
        }

        public async Task<BaseResponse<object>> UpdateRoleAsync(UpdateRoleCommandRequest model)
        {
            // Find the role by its identifier.
            Role? role = await _roleManager.FindByIdAsync(model.Id);
            //Role? role = await _roleManager.FindByNameAsync(model.Name);

            // If the role does not exist, throw exception.
            if (role == null)
                throw new Exception("Role not found.");

            // Update the role's name with the new value from the model.
            role.Name = model.Role.Name;
            //role.Name = model.NewName;

            // Attempt to update the role and capture the result.
            IdentityResult result = await _roleManager.UpdateAsync(role);

            // Create a message based on the result of the operation.
            string message = result.Succeeded ? "Role updated successfully." : string.Join("; ", result.Errors.Select(e => e.Description));

            // Create a message based on the result of the operation.
            return new()
            {
                IsSuccessful = result.Succeeded,
                Message = message
                //Message = result.Succeeded ? "Role updated successfully." : "An error occured while updating the role."
            };
        }
    }
}
