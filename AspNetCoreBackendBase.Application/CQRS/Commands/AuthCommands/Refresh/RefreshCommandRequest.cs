using AspNetCoreBackendBase.Application.DTOs;
using MediatR;

namespace AspNetCoreBackendBase.Application.CQRS.Commands.AuthCommands
{
    public class RefreshCommandRequest : IRequest<BaseResponse<RefreshCommandResponse>>
    {
        public required string RefreshToken { get; set; }
    }
}
