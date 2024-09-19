using AspNetCoreBackendBase.Application.DTOs;
using MediatR;

namespace AspNetCoreBackendBase.Application.CQRS.Commands.RoleCommands
{
    public class DeleteRoleCommandRequest : IRequest<BaseResponse<object>>
    {
        public required string Id { get; set; }
        // public required string Name { get; set; } name unique ?????
    }
}
