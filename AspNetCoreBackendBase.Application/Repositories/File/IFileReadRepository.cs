using File = AspNetCoreBackendBase.Domain.Entities.File;

namespace AspNetCoreBackendBase.Application.Repositories
{
    public interface IFileReadRepository : IReadRepository<File, Guid>
    {
    }
}
