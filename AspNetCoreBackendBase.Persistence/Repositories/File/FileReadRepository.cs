using AspNetCoreBackendBase.Application.Repositories;
using AspNetCoreBackendBase.Persistence.Context;
using File = AspNetCoreBackendBase.Domain.Entities.File;

namespace AspNetCoreBackendBase.Persistence.Repositories
{
    public class FileReadRepository : ReadRepository<File, Guid>, IFileReadRepository
    {
        public FileReadRepository(AspNetCoreBackendBaseDbContext context) : base(context)
        {
        }
    }
}
