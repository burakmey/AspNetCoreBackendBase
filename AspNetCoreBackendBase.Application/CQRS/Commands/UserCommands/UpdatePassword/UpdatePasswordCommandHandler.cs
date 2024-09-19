using AspNetCoreBackendBase.Application.DTOs;
using AspNetCoreBackendBase.Application.Services;
using MediatR;

namespace Application.Features.Commands.UserCommands
{
    public class UpdatePasswordCommandHandler : IRequestHandler<UpdatePasswordCommandRequest, BaseResponse<object>>
    {
        readonly IUserService _userService;

        public UpdatePasswordCommandHandler(IUserService userService)
        {
            _userService = userService;
        }
        public async Task<BaseResponse<object>> Handle(UpdatePasswordCommandRequest request, CancellationToken cancellationToken)
        {
           return await _userService.UpdatePasswordAsync(request);
        }
    }
}
