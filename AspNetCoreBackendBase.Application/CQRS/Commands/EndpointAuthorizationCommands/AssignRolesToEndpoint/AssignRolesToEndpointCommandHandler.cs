using AspNetCoreBackendBase.Application.DTOs;
using AspNetCoreBackendBase.Application.Services;
using MediatR;

namespace AspNetCoreBackendBase.Application.CQRS.Commands.EndpointAuthorizationCommands
{
    public class AssignRolesToEndpointCommandHandler : IRequestHandler<AssignRolesToEndpointCommandRequest, BaseResponse<object>>
    {
        readonly IEndpointAuthorizationService _endpointAuthorizationService;

        public AssignRolesToEndpointCommandHandler(IEndpointAuthorizationService endpointAuthorizationService)
        {
            _endpointAuthorizationService = endpointAuthorizationService;
        }

        public async Task<BaseResponse<object>> Handle(AssignRolesToEndpointCommandRequest request, CancellationToken cancellationToken)
        {
            return await _endpointAuthorizationService.AssignRolesToEndpointAsync(request);
        }
    }
}
