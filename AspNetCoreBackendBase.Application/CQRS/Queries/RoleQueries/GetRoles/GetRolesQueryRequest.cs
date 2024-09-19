using AspNetCoreBackendBase.Application.DTOs;
using MediatR;

namespace AspNetCoreBackendBase.Application.CQRS.Queries.RoleQueries
{
    public class GetRolesQueryRequest : IRequest<BaseResponse<GetRolesQueryResponse>>
    {
        public Pagination Pagination { get; set; } = new();
    }
}
