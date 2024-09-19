namespace AspNetCoreBackendBase.Application.CQRS.Queries.UserQueries
{
    public class GetUserRolesQueryResponse
    {
        public required string[] UserRoles { get; set; }
    }
}
