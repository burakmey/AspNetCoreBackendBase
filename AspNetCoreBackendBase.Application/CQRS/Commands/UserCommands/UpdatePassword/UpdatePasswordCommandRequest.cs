using AspNetCoreBackendBase.Application.DTOs;
using MediatR;

namespace Application.Features.Commands.UserCommands
{
    public class UpdatePasswordCommandRequest : IRequest<BaseResponse<object>>
    {
        public required string UserId { get; set; }
        public required string ResetToken { get; set; }
        public required string Password { get; set; }
        public required string ConfirmPassword { get; set; }
    }
}
