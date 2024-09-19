using AspNetCoreBackendBase.Application.DTOs;
using MediatR;

namespace AspNetCoreBackendBase.Application.CQRS.Commands.AuthCommands
{
    public class LoginCommandRequest : IRequest<BaseResponse<LoginCommandResponse>>
    {
        public required string UsernameOrEmail { get; set; }
        public required string Password { get; set; }
    }
}
