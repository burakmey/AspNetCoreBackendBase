using Application.Attributes;
using AspNetCoreBackendBase.API.Filters;
using AspNetCoreBackendBase.Application.CQRS.Commands.RoleCommands;
using AspNetCoreBackendBase.Application.CQRS.Queries.RoleQueries;
using AspNetCoreBackendBase.Application.DTOs;
using AspNetCoreBackendBase.Application.Enums;
using AspNetCoreBackendBase.Infrastructure.Services.WebApiServer;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AspNetCoreBackendBase.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize] //[Authorize("Bearer")] // Now uses JWT Bearer as the default scheme
    [ServiceFilter(typeof(RolePermissionFilter))]
    public class RoleController : ControllerBase
    {
        readonly IMediator _mediator;

        public RoleController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("create-role")]
        [AuthorizeDefinition(ActionType.Write, nameof(CreateRole), nameof(RoleController))]
        public async Task<IActionResult> CreateRole([FromBody] CreateRoleCommandRequest request)
        {
            BaseResponse<object> response = await _mediator.Send(request);
            return Ok(response);
        }

        [HttpDelete("delete-role/{id}")]
        [AuthorizeDefinition(ActionType.Delete, nameof(DeleteRole), nameof(RoleController))]
        public async Task<IActionResult> DeleteRole([FromRoute] DeleteRoleCommandRequest request)
        {
            BaseResponse<object> response = await _mediator.Send(request);
            return Ok(response);
        }

        [HttpGet("get-roles")]
        [AuthorizeDefinition(ActionType.Read, nameof(GetRoles), nameof(RoleController))]
        public async Task<IActionResult> GetRoles([FromQuery] Pagination pagination)
        {
            GetRolesQueryRequest request = new() { Pagination = pagination };
            BaseResponse<GetRolesQueryResponse> response = await _mediator.Send(request);
            return Ok(response);
        }

        [HttpPatch("update-role/{id}")]
        [AuthorizeDefinition(ActionType.Update, nameof(UpdateRole), nameof(RoleController))]
        public async Task<IActionResult> UpdateRole([FromRoute] string id, [FromBody] UpdateRoleFromBody requestBody )
        {
            UpdateRoleCommandRequest request = new() { Id = id, Role = requestBody };
            BaseResponse<object> response = await _mediator.Send(request);
            return Ok(response);
        }

        [HttpGet("get-authorize-definition-endpoints")]
        [AuthorizeDefinition(ActionType.Read, nameof(GetAuthorizeDefinitionEndpoints), nameof(RoleController))]
        public IActionResult GetAuthorizeDefinitionEndpoints()
        {
            WebApiServerService webApiServerService = new();
            var datas = webApiServerService.GetAuthorizeDefinitionEndpoints(typeof(Program));
            return Ok(datas);
        }
    }
}
