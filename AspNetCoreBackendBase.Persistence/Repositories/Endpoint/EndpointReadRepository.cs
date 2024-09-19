using AspNetCoreBackendBase.Application.Repositories;
using AspNetCoreBackendBase.Domain.Entities;
using AspNetCoreBackendBase.Persistence.Context;

namespace AspNetCoreBackendBase.Persistence.Repositories
{
    public class EndpointReadRepository : ReadRepository<Endpoint, int>, IEndpointReadRepository
    {
        public EndpointReadRepository(AspNetCoreBackendBaseDbContext context) : base(context)
        {
        }
    }
}