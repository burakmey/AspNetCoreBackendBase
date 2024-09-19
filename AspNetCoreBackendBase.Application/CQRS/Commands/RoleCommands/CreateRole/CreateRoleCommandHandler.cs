using AspNetCoreBackendBase.Application.DTOs;
using AspNetCoreBackendBase.Application.Services;
using MediatR;


namespace AspNetCoreBackendBase.Application.CQRS.Commands.RoleCommands
{
    public class CreateRoleCommandHandler : IRequestHandler<CreateRoleCommandRequest, BaseResponse<object>>
    {
        readonly IRoleService _roleService;

        public CreateRoleCommandHandler(IRoleService roleService)
        {
            _roleService = roleService;
        }

        public async Task<BaseResponse<object>> Handle(CreateRoleCommandRequest request, CancellationToken cancellationToken)
        {
            return await _roleService.CreateRoleAsync(request);
        }
    }
}
