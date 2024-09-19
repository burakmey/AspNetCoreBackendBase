using AspNetCoreBackendBase.Application.Repositories;
using AspNetCoreBackendBase.Domain.Entities;
using AspNetCoreBackendBase.Persistence.Context;

namespace AspNetCoreBackendBase.Persistence.Repositories
{
    public class EndpointWriteRepository : WriteRepository<Endpoint, int>, IEndpointWriteRepository
    {
        public EndpointWriteRepository(AspNetCoreBackendBaseDbContext context) : base(context)
        {
        }
    }
}