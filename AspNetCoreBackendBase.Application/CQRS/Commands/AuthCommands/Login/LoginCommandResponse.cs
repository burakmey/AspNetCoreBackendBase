using AspNetCoreBackendBase.Application.DTOs.Auth;

namespace AspNetCoreBackendBase.Application.CQRS.Commands.AuthCommands
{
    public class LoginCommandResponse
    {
        public required Token Token { get; set; }
    }
}
