using AspNetCoreBackendBase.Application.DTOs;
using MediatR;

namespace AspNetCoreBackendBase.Application.CQRS.Commands.RoleCommands
{
    public class CreateRoleCommandRequest : IRequest<BaseResponse<object>>
    {
        public required string Name { get; set; }
    }
}
