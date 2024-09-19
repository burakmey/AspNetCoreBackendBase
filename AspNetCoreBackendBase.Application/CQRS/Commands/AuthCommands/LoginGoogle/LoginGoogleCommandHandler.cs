using AspNetCoreBackendBase.Application.DTOs;
using AspNetCoreBackendBase.Application.Services;
using MediatR;

namespace AspNetCoreBackendBase.Application.CQRS.Commands.AuthCommands
{
    public class LoginGoogleCommandHandler : IRequestHandler<LoginGoogleCommandRequest, BaseResponse<LoginGoogleCommandResponse>>
    {
        readonly IExternalAuthentication _authService;

        public LoginGoogleCommandHandler(IExternalAuthentication authService)
        {
            _authService = authService;
        }

        public async Task<BaseResponse<LoginGoogleCommandResponse>> Handle(LoginGoogleCommandRequest request, CancellationToken cancellationToken)
        {
            return  await _authService.GoogleLoginAsync(request);
        }
    }
}
