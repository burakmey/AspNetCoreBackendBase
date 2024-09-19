using AspNetCoreBackendBase.Domain.Entities;

namespace AspNetCoreBackendBase.Application.Repositories
{
    public interface IEndpointWriteRepository : IWriteRepository<Endpoint, int>
    {
    }
}
