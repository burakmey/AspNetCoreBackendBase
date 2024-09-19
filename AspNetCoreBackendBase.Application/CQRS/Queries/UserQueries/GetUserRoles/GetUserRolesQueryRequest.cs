using AspNetCoreBackendBase.Application.DTOs;
using MediatR;

namespace AspNetCoreBackendBase.Application.CQRS.Queries.UserQueries
{
    public class GetUserRolesQueryRequest : IRequest<BaseResponse<GetUserRolesQueryResponse>>
    {
        public required string UserNameOrUserId { get; set; }
    }
}
