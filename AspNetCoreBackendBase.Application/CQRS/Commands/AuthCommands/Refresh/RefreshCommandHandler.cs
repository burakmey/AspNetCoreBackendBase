using AspNetCoreBackendBase.Application.DTOs;
using AspNetCoreBackendBase.Application.Services;
using MediatR;

namespace AspNetCoreBackendBase.Application.CQRS.Commands.AuthCommands

{
    public class RefreshCommandHandler : IRequestHandler<RefreshCommandRequest, BaseResponse<RefreshCommandResponse>>
    {
        readonly IInternalAuthentication _authService;

        public RefreshCommandHandler(IInternalAuthentication authService)
        {
            _authService = authService;
        }

        public async Task<BaseResponse<RefreshCommandResponse>> Handle(RefreshCommandRequest request, CancellationToken cancellationToken)
        {
            return await _authService.RefreshAsync(request);
        }
    }
}
