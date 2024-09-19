using AspNetCoreBackendBase.Application.DTOs;
using MediatR;

namespace AspNetCoreBackendBase.Application.CQRS.Commands.AuthCommands
{
    public class VerifyResetTokenCommandRequest : IRequest<BaseResponse<object>>
    {
        public required string UserId { get; set; }
        public required string ResetToken { get; set; }
    }
}
