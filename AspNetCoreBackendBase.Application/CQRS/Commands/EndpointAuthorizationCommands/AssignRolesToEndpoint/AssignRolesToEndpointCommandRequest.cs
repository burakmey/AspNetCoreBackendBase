using AspNetCoreBackendBase.Application.DTOs;
using MediatR;

namespace AspNetCoreBackendBase.Application.CQRS.Commands.EndpointAuthorizationCommands
{
    public class AssignRolesToEndpointCommandRequest : IRequest<BaseResponse<object>>
    {
        public required ICollection<string> Roles { get; set; }
        public required string Code { get; set; }
        public required string Route { get; set; }
    }
}
