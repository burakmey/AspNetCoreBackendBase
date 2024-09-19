using AspNetCoreBackendBase.Application.DTOs;
using MediatR;

namespace AspNetCoreBackendBase.Application.CQRS.Commands.AuthCommands
{
    public class LoginGoogleCommandRequest : IRequest<BaseResponse<LoginGoogleCommandResponse>>
    {
        public required string Id { get; set; }
        public required string IdToken { get; set; }
        public required string Name { get; set; }
        public required string Surname { get; set; }
        public required string Email { get; set; }
        public required string PhotoUrl { get; set; }
        public required string Provider { get; set; }
    }
}
