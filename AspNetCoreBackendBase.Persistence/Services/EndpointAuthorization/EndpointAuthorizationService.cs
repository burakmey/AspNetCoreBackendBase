using AspNetCoreBackendBase.Application.CQRS.Commands.EndpointAuthorizationCommands;
using AspNetCoreBackendBase.Application.CQRS.Queries.EndpointAuthorizationQueries;
using AspNetCoreBackendBase.Application.DTOs;
using AspNetCoreBackendBase.Application.Repositories;
using AspNetCoreBackendBase.Application.Services;
using AspNetCoreBackendBase.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Linq.Expressions;

namespace AspNetCoreBackendBase.Persistence.Services
{
    public class EndpointAuthorizationService : IEndpointAuthorizationService
    {
        readonly IEndpointReadRepository _endpointReadRepository;
        readonly IEndpointWriteRepository _endpointWriteRepository;
        readonly RoleManager<Role> _roleManager;

        public EndpointAuthorizationService(IEndpointReadRepository endpointReadRepository, IEndpointWriteRepository endpointWriteRepository, RoleManager<Role> roleManager)
        {
            _endpointReadRepository = endpointReadRepository;
            _endpointWriteRepository = endpointWriteRepository;
            _roleManager = roleManager;
        }

        public async Task<BaseResponse<object>> AssignRolesToEndpointAsync(AssignRolesToEndpointCommandRequest model)
        {
            // Validate roles from the model to ensure they exist.
            var existingRoles = await _roleManager.Roles
                .Where(r => model.Roles.Contains(r.Name!))
                .Select(r => r.Name)
                .ToListAsync();

            if (existingRoles.Count != model.Roles.Count)
            {
                // If some roles are missing, throw exception.
                var missingRoles = model.Roles.Except(existingRoles).ToList();
                throw new Exception($"Roles not found: {string.Join(", ", missingRoles)}");
                //throw new RolesNotFoundException($"Roles not found: {string.Join(", ", missingRoles)}");
            }

            // Define the condition to find the endpoint.
            Expression<Func<Endpoint, bool>> condition = e => e.Code == model.Code && e.Route.Name == model.Route;

            // Create the query with Include for navigation properties.
            IQueryable<Endpoint> query = _endpointReadRepository.GetWhere(condition)
                .Include(e => e.Route)
                .Include(e => e.RoleEndpoints)
                .ThenInclude(re => re.Role);

            Endpoint? endpoint = await query.FirstOrDefaultAsync();

            // If the endpoint does not exist, throw exception.
            if (endpoint == null)
                throw new Exception("Endpoint not found");
            //if (endpoint == null)
            //    throw new EndpointNotFoundException($"Endpoint with code '{model.Code}' and route '{model.Route}' was not found.");

            // Get the role names already assigned to this endpoint.
            var existingRoleNames = endpoint.RoleEndpoints.Select(re => re.Role.Name).ToList();

            // Fetch roles from the database that need to be assigned excluding already assigned roles.
            var rolesToAdd = await _roleManager.Roles
                .Where(r => model.Roles.Contains(r.Name!) && !existingRoleNames.Contains(r.Name))    
                .ToListAsync();

            // Add only new roles.
            foreach (var role in rolesToAdd)
                endpoint.RoleEndpoints.Add(new RoleEndpoint { Role = role, Endpoint = endpoint });


            // Save changes to the database.
            await _endpointWriteRepository.SaveAsync();

            // Return a successful response.
            return new BaseResponse<object>
            {
                IsSuccessful = true,
                Message = "Roles assigned successfully."
            };
        }

        public async Task<BaseResponse<GetRolesForEndpointQueryResponse>> GetRolesForEndpointAsync(GetRolesForEndpointQueryRequest model)
        {
            // Define the condition to find the endpoint.
            Expression<Func<Endpoint, bool>> condition = e => e.Code == model.Code && e.Route!.Name == model.Route;

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

            //if (endpoint == null)
            //    throw new EndpointNotFoundException($"Endpoint with code '{model.Code}' and route '{model.Route}' not found.");

            // Extract role names from RoleEndpoints
            List<string> roles = endpoint.RoleEndpoints
                .Select(re => re.Role!.Name)
                .Where(name => name != null)  // Filter out any null names
                .ToList()!;                   // Convert to list and assert non-null

            // Return roles in the response
            return new BaseResponse<GetRolesForEndpointQueryResponse>
            {
                IsSuccessful = true,
                Data = new() { Roles = roles }
            };
        }
    }
}
