using AspNetCoreBackendBase.Application.DTOs;
using AspNetCoreBackendBase.Application.Services;
using MediatR;

namespace AspNetCoreBackendBase.Application.CQRS.Commands.AuthCommands
{
    public class LoginCommandHandler : IRequestHandler<LoginCommandRequest, BaseResponse<LoginCommandResponse>>
    {
        readonly IInternalAuthentication _authService;

        public LoginCommandHandler(IInternalAuthentication authService)
        {
            _authService = authService;
        }

        public async Task<BaseResponse<LoginCommandResponse>> Handle(LoginCommandRequest request, CancellationToken cancellationToken)
        {
            return await _authService.LoginAsync(request);
        }
    }
}
