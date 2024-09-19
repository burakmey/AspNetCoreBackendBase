using Application.Features.Commands.AuthCommands;
using Application.Features.Commands.UserCommands;
using Application.Helpers;
using AspNetCoreBackendBase.Application.CQRS.Commands.UserCommands;
using AspNetCoreBackendBase.Application.CQRS.Queries.UserQueries;
using AspNetCoreBackendBase.Application.DTOs;
using AspNetCoreBackendBase.Application.Exceptions;
using AspNetCoreBackendBase.Application.Repositories;
using AspNetCoreBackendBase.Application.Services;
using AspNetCoreBackendBase.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace AspNetCoreBackendBase.Persistence.Services
{
    public class UserService : IUserService
    {
        readonly UserManager<User> _userManager;
        readonly RoleManager<Role> _roleManager;
        readonly IEndpointReadRepository _endpointReadRepository;

        public UserService(UserManager<User> userManager, IEndpointReadRepository endpointReadRepository, RoleManager<Role> roleManager)
        {
            _userManager = userManager;
            _endpointReadRepository = endpointReadRepository;
            _roleManager = roleManager;
        }

        public async Task<BaseResponse<object>> CreateUserAsync(RegisterCommandRequest model)
        {
            // Create a new user with the provided details and attempt to create the user with the specified password.
            IdentityResult result = await _userManager.CreateAsync(new()
            {
                Id = Guid.NewGuid(),
                Name = model.Name,
                Surname = model.Surname,
                Email = model.Email,
                UserName = model.UserName,
            }, model.Password);

            // Check if the user creation was successful.
            if (result.Succeeded)
                return new()
                {
                    IsSuccessful = true,
                    Data = null,
                    Message = "User registered successfully."
                };
            else
            {
                // Aggregate errors from the result and include them in the exception.
                var errorMessages = string.Join("; ", result.Errors.Select(e => e.Description));
                throw new CreateUserException(errorMessages);
            }

        }

        public async Task<BaseResponse<object>> UpdatePasswordAsync(UpdatePasswordCommandRequest model)
        {
            // Check if the password and confirmation password match.
            if (!model.Password.Equals(model.ConfirmPassword))
            {
                throw new PasswordResetFailedException("Passwords do not match. Please ensure both password fields are identical.");
            }

            // Retrieve the user based on the provided user ID.
            User user = await GetUserAsync(model.UserId);

            // Decode the reset token from the request model.
            string resetToken = model.ResetToken.UrlDecode();

            // Attempt to reset the user's password.
            IdentityResult result = await _userManager.ResetPasswordAsync(user, resetToken, model.Password);
            if (!result.Succeeded)
            {
                // Aggregate errors from the result and include them in the exception.
                var errorMessages = string.Join("; ", result.Errors.Select(e => e.Description));
                throw new PasswordResetFailedException(errorMessages);
            }

            // Update the user's security stamp to invalidate old tokens.
            await _userManager.UpdateSecurityStampAsync(user);

            // Return a successful response.
            return new()
            {
                IsSuccessful = true,
                Message = "Password reset successfully."
            };
        }

        public async Task<BaseResponse<GetUserRolesQueryResponse>> GetUserRolesAsync(GetUserRolesQueryRequest model)
        {
            // Retrieve all roles for the user based on the provided username or user ID.
            var userRoles = await GetAllRolesForUserAsync(model.UserNameOrUserId);

            // Return the response with the user roles.
            return new()
            {
                IsSuccessful = true,
                Data = new() { UserRoles = userRoles },
                // Optionally, include a message or other relevant information
                Message = userRoles.Length == 0 ? "No roles found for the user." : "Roles retrieved successfully."
            };
        }

        public async Task<BaseResponse<object>> AssignUserRolesAsync(AssignUserRolesCommandRequest model)
        {
            // Retrieve the user based on the provided username or user ID.
            User user = await GetUserAsync(model.UserNameOrUserId);

            // Get the current roles assigned to the user.
            var currentRoles = await _userManager.GetRolesAsync(user);

            // Determine which roles need to be added and which need to be removed.
            var rolesToAdd = model.Roles.Except(currentRoles).ToList();
            var rolesToRemove = currentRoles.Except(model.Roles).ToList();

            // Validate that roles to add exist in the system.
            var nonExistentRoles = rolesToAdd.Except(_roleManager.Roles.Select(r => r.Name)).ToList();
            if (nonExistentRoles.Count != 0)
            {
                string errorMessage = "The following roles do not exist: " + string.Join(", ", nonExistentRoles);
                return new()
                {
                    IsSuccessful = false,
                    Message = errorMessage
                };
            }

            // Add new roles to the user if any.
            if (rolesToAdd.Count != 0)
            {
                var addResult = await _userManager.AddToRolesAsync(user, rolesToAdd);
                if (!addResult.Succeeded)
                {
                    string errorMessage = "Failed to add some roles: " + string.Join(", ", addResult.Errors.Select(e => e.Description));
                    return new()
                    {
                        IsSuccessful = false,
                        Message = errorMessage
                    };
                }
            }

            // Remove roles from the user if any.
            if (rolesToRemove.Count != 0)
            {
                var removeResult = await _userManager.RemoveFromRolesAsync(user, rolesToRemove);
                if (!removeResult.Succeeded)
                {
                    string errorMessage = "Failed to remove some roles: " + string.Join(", ", removeResult.Errors.Select(e => e.Description));
                    return new()
                    {
                        IsSuccessful = false,
                        Message = errorMessage
                    };
                }
            }

            // Return a successful response if all operations were successful.
            return new()
            {
                IsSuccessful = true,
                Message = "Roles assigned successfully."
            };
        }

        public async Task<string[]> GetAllRolesForUserAsync(string userIdOrUserName)
        {
            // Retrieve the user based on the provided user ID or username.
            User user = await GetUserAsync(userIdOrUserName);

            // Get all roles assigned to the user and return them as an array.
            var userRoles = await _userManager.GetRolesAsync(user);
            return userRoles.ToArray();
        }

        public async Task<User> GetUserAsync(string userIdOrUserName)
        {
            User? user;

            // Check if the input is a valid GUID.
            if (Guid.TryParse(userIdOrUserName, out _))
                // If it's a GUID, find by ID.
                user = await _userManager.FindByIdAsync(userIdOrUserName);
            else
                // If it's not a GUID, find by name.
                user = await _userManager.FindByNameAsync(userIdOrUserName);

            // If the user is not found, throw exception.
            if (user == null)
                throw new UserNotFoundException("User not found!");

            return user;
        }

        public async Task<bool> HasRolePermissionForEndpointAsync(string userName, string code, string route)
        {
            // Retrieve all roles for the user based on the provided username.
            var userRoles = await GetAllRolesForUserAsync(userName);

            // Return false if the user has no roles.
            if (userRoles.Length == 0)
                return false;

            // Define the condition to find the endpoint.
            Expression<Func<Endpoint, bool>> condition = e => e.Code == code && e.Route!.Name == route;

            // Create the query with Include for navigation properties.
            IQueryable<Endpoint> query = _endpointReadRepository.GetWhere(condition, tracking: false)
                .Include(e => e.Route)
                .Include(e => e.RoleEndpoints)
                .ThenInclude(re => re.Role);

            // Fetch the endpoint from the database.
            Endpoint? endpoint = await query.FirstOrDefaultAsync();

            // If the endpoint does not exist, throw exception.
            if (endpoint == null)
                throw new Exception("Endpoint not found");
            // throw new EndpointNotFoundException($"Endpoint with code '{model.Code}' and route '{model.Route}' not found.");


            // Get the role names associated with the endpoint.
            List<string> endpointRoles = endpoint.RoleEndpoints
                .Select(re => re.Role.Name)
                .ToList()!;

            return userRoles
            // Find the common elements between userRoles and endpointRoles.
            .Intersect(endpointRoles)
            // Check if there is at least one common role between userRoles and endpointRoles.
            // If there is any intersection, Any() will return true; otherwise, it will return false.
            .Any();
        }
    }
}
