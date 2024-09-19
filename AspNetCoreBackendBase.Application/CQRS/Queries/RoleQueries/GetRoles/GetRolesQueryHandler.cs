using AspNetCoreBackendBase.Application.DTOs;
using AspNetCoreBackendBase.Application.Services;
using MediatR;

namespace AspNetCoreBackendBase.Application.CQRS.Queries.RoleQueries
{

    public class GetRolesQueryHandler : IRequestHandler<GetRolesQueryRequest, BaseResponse<GetRolesQueryResponse>>
    {
        readonly IRoleService _roleService;

        public GetRolesQueryHandler(IRoleService roleService)
        {
            _roleService = roleService;
        }

        public Task<BaseResponse<GetRolesQueryResponse>> Handle(GetRolesQueryRequest request, CancellationToken cancellationToken)
        {
            return Task.FromResult(_roleService.GetRoles(request));
        }
    }
}
