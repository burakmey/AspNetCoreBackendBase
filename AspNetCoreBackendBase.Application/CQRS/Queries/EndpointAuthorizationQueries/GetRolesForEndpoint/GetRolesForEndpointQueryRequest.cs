using AspNetCoreBackendBase.Application.DTOs;
using MediatR;

namespace AspNetCoreBackendBase.Application.CQRS.Queries.EndpointAuthorizationQueries
{
    public class GetRolesForEndpointQueryRequest : IRequest<BaseResponse<GetRolesForEndpointQueryResponse>>
    {
        public required string Code { get; set; }
        public required string Route { get; set; }
    }
}
