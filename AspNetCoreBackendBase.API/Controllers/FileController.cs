using AspNetCoreBackendBase.Application.Repositories;
using AspNetCoreBackendBase.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace AspNetCoreBackendBase.API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class FileController : ControllerBase
    {
        readonly IFileReadRepository _fileReadRepository;
        readonly IFileWriteRepository _fileWriteRepository;
        readonly IStorageService _storageService;

        public FileController(IFileReadRepository fileReadRepository, IFileWriteRepository fileWriteRepository, IStorageService storageService)
        {
            _fileReadRepository = fileReadRepository;
            _fileWriteRepository = fileWriteRepository;
            _storageService = storageService;
        }

        [HttpPost]
        public async Task<IActionResult> Upload()
        {
            List<(string fileName, string pathOrContainerName)> result = await _storageService.UploadAsync("files", Request.Form.Files);
            await _fileWriteRepository.AddRangeAsync(
                result.Select(file => new Domain.Entities.File { FileName = file.fileName, Path = file.pathOrContainerName, StorageId = _storageService.StorageTypeId }).ToList());
            await _fileWriteRepository.SaveAsync();
            return Ok("");
        }
    }
}
