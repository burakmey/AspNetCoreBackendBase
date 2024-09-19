using AspNetCoreBackendBase.Application.DTOs;
using AspNetCoreBackendBase.Application.Services;
using MediatR;


namespace AspNetCoreBackendBase.Application.CQRS.Commands.RoleCommands
{
    public class DeleteRoleCommandHandler : IRequestHandler<DeleteRoleCommandRequest, BaseResponse<object>>
    {
        readonly IRoleService _roleService;

        public DeleteRoleCommandHandler(IRoleService roleService)
        {
            _roleService = roleService;
        }

        public async Task<BaseResponse<object>> Handle(DeleteRoleCommandRequest request, CancellationToken cancellationToken)
        {
            return await _roleService.DeleteRoleAsync(request);
        }
    }
}
