using AspNetCoreBackendBase.Application.DTOs;
using AspNetCoreBackendBase.Application.Services;
using MediatR;

namespace AspNetCoreBackendBase.Application.CQRS.Queries.UserQueries.GetUserRoles
{
    public class GetUserRolesQueryHandler : IRequestHandler<GetUserRolesQueryRequest, BaseResponse<GetUserRolesQueryResponse>>
    {
        readonly IUserService _userService;

        public GetUserRolesQueryHandler(IUserService userService)
        {
            _userService = userService;
        }

        public async Task<BaseResponse<GetUserRolesQueryResponse>> Handle(GetUserRolesQueryRequest request, CancellationToken cancellationToken)
        {
            return await _userService.GetUserRolesAsync(request);
        }
    }
}
