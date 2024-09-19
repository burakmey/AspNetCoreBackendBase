using Application.Attributes;
using Application.Features.Commands.UserCommands;
using AspNetCoreBackendBase.Application.CQRS.Commands.UserCommands;
using AspNetCoreBackendBase.Application.CQRS.Queries.UserQueries;
using AspNetCoreBackendBase.Application.DTOs;
using AspNetCoreBackendBase.Application.Enums;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AspNetCoreBackendBase.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UserController : ControllerBase
    {
        readonly IMediator _mediator;

        public UserController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("update-password")]
        [AllowAnonymous]
        public async Task<IActionResult> UpdatePassword([FromBody] UpdatePasswordCommandRequest request)
        {
            BaseResponse<object> response = await _mediator.Send(request);
            return Ok(response);
        }

        [HttpGet("get-user-roles/{UserNameOrUserId}")]
        [AuthorizeDefinition(ActionType.Read, nameof(GetUserRoles),  nameof(UserController))]
        public async Task<IActionResult> GetUserRoles([FromRoute] GetUserRolesQueryRequest request)
        {
            BaseResponse<GetUserRolesQueryResponse> response = await _mediator.Send(request);
            return Ok(response);
        }

        [HttpPost("assign-user-roles")]
        [AuthorizeDefinition(ActionType.Write, nameof(AssignUserRoles), nameof(UserController))]
        public async Task<IActionResult> AssignUserRoles([FromBody] AssignUserRolesCommandRequest request)
        {
            BaseResponse<object> response = await _mediator.Send(request);
            return Ok(response);
        }
    }
}
