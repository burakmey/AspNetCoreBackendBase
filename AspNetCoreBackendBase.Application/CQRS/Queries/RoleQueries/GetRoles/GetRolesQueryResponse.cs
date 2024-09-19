namespace AspNetCoreBackendBase.Application.CQRS.Queries.RoleQueries
{
    public class GetRolesQueryResponse
    {
        public required ICollection<string> Roles { get; set; }
    }
}
