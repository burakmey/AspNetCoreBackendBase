using AspNetCoreBackendBase.Application;
using AspNetCoreBackendBase.Application.Services;
using Azure.Storage.Blobs;
using Microsoft.AspNetCore.Http;

namespace AspNetCoreBackendBase.Infrastructure.Services
{
    public class AzureStorageService : Storage, IAzureStorageService
    {
        readonly BlobServiceClient _blobServiceClient;
        BlobContainerClient? _blobContainerClient;

        public AzureStorageService()
        {
            _blobServiceClient = new(Configuration.GetStorageAzure);
        }

        public async Task<bool> DeleteAsync(string pathOrContainerName, string fileName)
        {
            _blobContainerClient = _blobServiceClient.GetBlobContainerClient(pathOrContainerName);
            BlobClient blobClient = _blobContainerClient.GetBlobClient(fileName);
            return await blobClient.DeleteIfExistsAsync();
        }

        public List<string> GetFiles(string pathOrContainerName)
        {
            _blobContainerClient = _blobServiceClient.GetBlobContainerClient(pathOrContainerName);
            return _blobContainerClient.GetBlobs().Select(b => b.Name).ToList();
        }

        public new bool HasFile(string pathOrContainerName, string fileName)
        {
            _blobContainerClient = _blobServiceClient.GetBlobContainerClient(pathOrContainerName);
            return _blobContainerClient.GetBlobs().Any(b => b.Name == fileName);
        }

        public async Task<List<(string fileName, string pathOrContainerName)>> UploadAsync(string pathOrContainerName, IFormFileCollection files)
        {
            _blobContainerClient = _blobServiceClient.GetBlobContainerClient(pathOrContainerName);
            await _blobContainerClient.CreateIfNotExistsAsync();
            List<(string fileName, string pathOrContainerName)> datas = [];

            foreach (IFormFile file in files)
            {
                string fileName = await FileRenameAsync(pathOrContainerName, file.FileName, HasFile);
                BlobClient blobClient = _blobContainerClient.GetBlobClient(fileName);
                await blobClient.UploadAsync(file.OpenReadStream());
                datas.Add((fileName, $"{pathOrContainerName}/{fileName}"));
            }
            return datas;
        }
    }
}
