namespace AspNetCoreBackendBase.Application.CQRS.Queries.EndpointAuthorizationQueries
{
    public class GetRolesForEndpointQueryResponse
    {
        public required ICollection<string> Roles { get; set; }
    }
}
