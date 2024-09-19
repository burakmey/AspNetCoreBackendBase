using AspNetCoreBackendBase.Application.DTOs;
using AspNetCoreBackendBase.Application.Services;
using MediatR;

namespace AspNetCoreBackendBase.Application.CQRS.Commands.AuthCommands
{
    public class VerifyResetTokenCommandHandler : IRequestHandler<VerifyResetTokenCommandRequest, BaseResponse<object>>
    {
        readonly IAuthService _authService;

        public VerifyResetTokenCommandHandler(IAuthService authService)
        {
            _authService = authService;
        }

        public async Task<BaseResponse<object>> Handle(VerifyResetTokenCommandRequest request, CancellationToken cancellationToken)
        {
            return await _authService.VerifyResetTokenAsync(request);
        }
    }
}
