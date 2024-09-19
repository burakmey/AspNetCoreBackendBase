using AspNetCoreBackendBase.Application.DTOs;
using MediatR;

namespace AspNetCoreBackendBase.Application.CQRS.Commands.AuthCommands
{
    public class ResetPasswordCommandRequest : IRequest<BaseResponse<object>>
    {
        public required string Email { get; set; }
    }
}
