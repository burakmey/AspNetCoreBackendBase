using AspNetCoreBackendBase.Application.DTOs;
using MediatR;

namespace Application.Features.Commands.AuthCommands
{
    public class RegisterCommandRequest : IRequest<BaseResponse<object>>
    {
        public required string Name { get; set; }
        public required string Surname { get; set; }
        public required string Email { get; set; }
        public required string UserName { get; set; }
        public required string Password { get; set; }
        public required string ConfirmPassword { get; set; }
    }
}
