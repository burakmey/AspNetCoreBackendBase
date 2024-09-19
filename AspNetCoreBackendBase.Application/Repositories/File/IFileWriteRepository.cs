using File = AspNetCoreBackendBase.Domain.Entities.File;

namespace AspNetCoreBackendBase.Application.Repositories
{
    public interface IFileWriteRepository : IWriteRepository<File, Guid>
    {
    }
}
