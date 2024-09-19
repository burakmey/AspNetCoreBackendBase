using AspNetCoreBackendBase.Application.Repositories;
using AspNetCoreBackendBase.Persistence.Context;
using File = AspNetCoreBackendBase.Domain.Entities.File;

namespace AspNetCoreBackendBase.Persistence.Repositories
{
    public class FileWriteRepository : WriteRepository<File, Guid>, IFileWriteRepository
    {
        public FileWriteRepository(AspNetCoreBackendBaseDbContext context) : base(context)
        {
        }
    }
}
