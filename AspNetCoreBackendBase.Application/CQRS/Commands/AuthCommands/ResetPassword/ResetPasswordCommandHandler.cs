using AspNetCoreBackendBase.Application.DTOs;
using AspNetCoreBackendBase.Application.Services;
using MediatR;

namespace AspNetCoreBackendBase.Application.CQRS.Commands.AuthCommands
{
    public class ResetPasswordCommandHandler : IRequestHandler<ResetPasswordCommandRequest, BaseResponse<object>>
    {
        readonly IAuthService _authService;

        public ResetPasswordCommandHandler(IAuthService authService)
        {
            _authService = authService;
        }

        public async Task<BaseResponse<object>> Handle(ResetPasswordCommandRequest request, CancellationToken cancellationToken)
        {
            return await _authService.ResetPasswordAsync(request);
        }
    }
}
