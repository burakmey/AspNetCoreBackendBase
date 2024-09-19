using AspNetCoreBackendBase.Application.DTOs;
using AspNetCoreBackendBase.Application.Services;
using MediatR;

namespace Application.Features.Commands.AuthCommands
{
    public class RegisterCommandHandler : IRequestHandler<RegisterCommandRequest, BaseResponse<object>>
    {
        readonly IUserService _userService;

        public RegisterCommandHandler(IUserService userService)
        {
            _userService = userService;
        }

        public async Task<BaseResponse<object>> Handle(RegisterCommandRequest request, CancellationToken cancellationToken)
        {
            return await _userService.CreateUserAsync(request);
        }
    }
}