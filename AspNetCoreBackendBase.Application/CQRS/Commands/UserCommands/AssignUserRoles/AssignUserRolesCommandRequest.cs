using AspNetCoreBackendBase.Application.DTOs;
using MediatR;

namespace AspNetCoreBackendBase.Application.CQRS.Commands.UserCommands
{
    public class AssignUserRolesCommandRequest : IRequest<BaseResponse<object>>
    {
        public required string UserNameOrUserId { get; set; }
        public required string[] Roles { get; set; }
    }
}
