using AspNetCoreBackendBase.Application.DTOs;
using MediatR;

namespace AspNetCoreBackendBase.Application.CQRS.Commands.RoleCommands
{
    public class UpdateRoleCommandRequest : IRequest<BaseResponse<object>>
    {
        public required string Id { get; set; }
        public required UpdateRoleFromBody Role { get; set; }

        //public required string Name { get; set; }
        //public required string NewName { get; set; }
    }
}
