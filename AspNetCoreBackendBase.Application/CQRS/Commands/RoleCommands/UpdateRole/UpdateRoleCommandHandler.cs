using AspNetCoreBackendBase.Application.DTOs;
using AspNetCoreBackendBase.Application.Services;
using MediatR;

namespace AspNetCoreBackendBase.Application.CQRS.Commands.RoleCommands
{
    public class UpdateRoleCommandHandler : IRequestHandler<UpdateRoleCommandRequest, BaseResponse<object>>
    {
        readonly IRoleService _roleService;

        public UpdateRoleCommandHandler(IRoleService roleService)
        {
            _roleService = roleService;
        }
        public async Task<BaseResponse<object>> Handle(UpdateRoleCommandRequest request, CancellationToken cancellationToken)
        {
            return await _roleService.UpdateRoleAsync(request);
        }
    }
}
