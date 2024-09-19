using AspNetCoreBackendBase.Application.Services;
using AspNetCoreBackendBase.Domain.Enums;
using Microsoft.AspNetCore.Http;

namespace AspNetCoreBackendBase.Infrastructure.Services
{
    public class StorageService : IStorageService
    {
        readonly IStorage _storage;

        public StorageService(IStorage storage)
        {
            _storage = storage;
        }

        public string StorageName => _storage.GetType().Name;

        public int StorageTypeId
        {
            get
            {
                string s = "StorageService";
                string fullStorageName = StorageName;
                if (fullStorageName == $"AWS{s}")
                    return (int)StorageType.AWS;
                else if (fullStorageName == $"Azure{s}")
                    return (int)StorageType.Azure;
                else if (fullStorageName == $"Local{s}")
                    return (int)StorageType.Local;
                else
                    return 0;
            }
        }

        public async Task<bool> DeleteAsync(string pathOrContainerName, string fileName)
        {
            return await _storage.DeleteAsync(pathOrContainerName, fileName);
        }

        public List<string> GetFiles(string pathOrContainerName)
        {
            return _storage.GetFiles(pathOrContainerName);
        }

        public bool HasFile(string pathOrContainerName, string fileName)
        {
            return _storage.HasFile(pathOrContainerName, fileName);
        }

        public Task<List<(string fileName, string pathOrContainerName)>> UploadAsync(string pathOrContainerName, IFormFileCollection files)
        {
            return _storage.UploadAsync(pathOrContainerName, files);
        }
    }
}
