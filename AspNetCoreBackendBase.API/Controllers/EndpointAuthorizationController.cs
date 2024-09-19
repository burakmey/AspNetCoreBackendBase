using AspNetCoreBackendBase.Application.CQRS.Commands.EndpointAuthorizationCommands;
using AspNetCoreBackendBase.Application.CQRS.Queries.EndpointAuthorizationQueries;
using AspNetCoreBackendBase.Application.DTOs;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AspNetCoreBackendBase.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class EndpointAuthorizationController : ControllerBase
    {
        readonly IMediator _mediator;

        public EndpointAuthorizationController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("assign-roles-to-endpoint")]
        public async Task<IActionResult> AssignRolesToEndpoint([FromBody] AssignRolesToEndpointCommandRequest request)
        {
            BaseResponse <object> response = await _mediator.Send(request);
            return Ok(response);
        }

        [HttpPost("get-roles-for-endpoint")]
        public async Task<IActionResult> GetRolesForEndpoint([FromBody] GetRolesForEndpointQueryRequest request)
        {
            BaseResponse<GetRolesForEndpointQueryResponse> response = await _mediator.Send(request);
            return Ok(response);
        }
    }
}
