using Application.Features.Commands.AuthCommands;
using AspNetCoreBackendBase.Application.CQRS.Commands.AuthCommands;
using AspNetCoreBackendBase.Application.DTOs;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace AspNetCoreBackendBase.API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        readonly IMediator _mediator;

        public AuthController(IMediator mediator) 
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterCommandRequest request)
        {
            BaseResponse<object> response = await _mediator.Send(request);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginCommandRequest request)
        {
            BaseResponse<LoginCommandResponse> response = await _mediator.Send(request);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> Refresh([FromBody] RefreshCommandRequest request)
        {
            BaseResponse<RefreshCommandResponse> response = await _mediator.Send(request);
            return Ok(response);
        }

        [HttpPost("login-google")]
        public async Task<IActionResult> LoginGoogle(LoginGoogleCommandRequest request)
        {
            BaseResponse<LoginGoogleCommandResponse> response = await _mediator.Send(request);
            return Ok(response);
        }

        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordCommandRequest request)
        {
            BaseResponse<object> response = await _mediator.Send(request);
            return Ok(response);
        }

        [HttpPost("verify-reset-token")]
        public async Task<IActionResult> VerifyResetToken([FromBody] VerifyResetTokenCommandRequest request)
        {
            BaseResponse<object> response = await _mediator.Send(request);
            return Ok(response);
        }
    }
}
