using AspNetCoreBackendBase.Application.Services;
using Microsoft.AspNetCore.Http;

namespace AspNetCoreBackendBase.Infrastructure.Services
{
    public class LocalStorageService : Storage, ILocalStorageService
    {
        public Task<List<(string fileName, string pathOrContainerName)>> UploadAsync(string pathName, IFormFileCollection files)
        {
            throw new NotImplementedException();
        }
        public Task<bool> DeleteAsync(string pathName, string fileName)
        {
            throw new NotImplementedException();
        }

        public List<string> GetFiles(string pathName)
        {
            throw new NotImplementedException();
        }

        public new bool HasFile(string pathName, string fileName)
        {
            throw new NotImplementedException();
        }
    }
}
