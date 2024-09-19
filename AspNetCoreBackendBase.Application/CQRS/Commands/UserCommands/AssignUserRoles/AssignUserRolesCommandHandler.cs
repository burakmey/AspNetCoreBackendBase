using AspNetCoreBackendBase.Application.DTOs;
using AspNetCoreBackendBase.Application.Services;
using MediatR;

namespace AspNetCoreBackendBase.Application.CQRS.Commands.UserCommands.AssignUserRoles
{
    internal class AssignUserRolesCommandHandler : IRequestHandler<AssignUserRolesCommandRequest, BaseResponse<object>>
    {
        readonly IUserService _userService;
        public AssignUserRolesCommandHandler(IUserService userService)
        {
            _userService = userService;
        }

        public async Task<BaseResponse<object>> Handle(AssignUserRolesCommandRequest request, CancellationToken cancellationToken)
        {
            return await _userService.AssignUserRolesAsync(request);
        }
    }
}
