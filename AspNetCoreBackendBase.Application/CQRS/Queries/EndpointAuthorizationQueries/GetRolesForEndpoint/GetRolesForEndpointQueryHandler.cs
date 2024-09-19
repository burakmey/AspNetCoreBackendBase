using AspNetCoreBackendBase.Application.DTOs;
using AspNetCoreBackendBase.Application.Services;
using MediatR;

namespace AspNetCoreBackendBase.Application.CQRS.Queries.EndpointAuthorizationQueries
{
    public class GetRolesForEndpointQueryHandler : IRequestHandler<GetRolesForEndpointQueryRequest, BaseResponse<GetRolesForEndpointQueryResponse>>
    {
        readonly IEndpointAuthorizationService _endpointAuthorizationService;

        public GetRolesForEndpointQueryHandler(IEndpointAuthorizationService endpointAuthorizationService)
        {
            _endpointAuthorizationService = endpointAuthorizationService;
        }
        public async Task<BaseResponse<GetRolesForEndpointQueryResponse>> Handle(GetRolesForEndpointQueryRequest request, CancellationToken cancellationToken)
        {
            return await _endpointAuthorizationService.GetRolesForEndpointAsync(request);
        }
    }
}
