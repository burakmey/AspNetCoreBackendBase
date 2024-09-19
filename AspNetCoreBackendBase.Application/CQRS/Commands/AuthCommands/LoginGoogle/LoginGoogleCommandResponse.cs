using AspNetCoreBackendBase.Application.DTOs.Auth;

namespace AspNetCoreBackendBase.Application.CQRS.Commands.AuthCommands
{
    public class LoginGoogleCommandResponse
    {
        public required Token Token { get; set; }
    }
}
