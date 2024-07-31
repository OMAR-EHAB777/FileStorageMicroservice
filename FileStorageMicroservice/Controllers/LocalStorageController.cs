using FileStorageMicroservice.Repositories;
using FileStorageMicroservice.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace FileStorageMicroservice.Controllers
{
    [Route("api/localstorage")]
    [ApiController]
    public class LocalStorageController : ControllerBase
    {
        private readonly IStorageService _localFileStorageService;
        private readonly IFileMetadataRepository _metadataRepository;

        public LocalStorageController(IStorageService localFileStorageService, IFileMetadataRepository metadataRepository)
        {
            _localFileStorageService = localFileStorageService;
            _metadataRepository = metadataRepository;
        }

        [HttpPost("upload")]
        public async Task<IActionResult> UploadFile(IFormFile file)
        {
            var fileId = await _localFileStorageService.UploadFileAsync(file);
            return Ok(new { FileId = fileId });
        }

        [HttpGet("download/{fileId}")]
        public async Task<IActionResult> DownloadFile(string fileId)
        {
            var (fileStream, contentType) = await _localFileStorageService.DownloadFileAsync(fileId);

            if (fileStream == null)
            {
                return NotFound();
            }

            var fileMetadata = await _metadataRepository.GetFileMetadataAsync(fileId);
            var fileName = fileMetadata?.FileName ?? "downloadedFile";

            return File(fileStream, contentType, fileName);
        }


        [HttpDelete("delete/{fileId}")]
        public async Task<IActionResult> DeleteFile(string fileId)
        {
            var result = await _localFileStorageService.DeleteFileAsync(fileId);
            if (result)
            {
                return NoContent();
            }
            return NotFound();
        }
    }
}
