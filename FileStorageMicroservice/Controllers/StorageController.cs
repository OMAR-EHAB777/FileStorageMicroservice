using FileStorageMicroservice.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace FileStorageMicroservice.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StorageController : ControllerBase
    {
        private readonly IStorageService _storageService;

        public StorageController(IStorageService storageService)
        {
            _storageService = storageService;
        }

        [HttpPost("upload")]
        public async Task<IActionResult> UploadFile(IFormFile file)
        {
            var fileId = await _storageService.UploadFileAsync(file);
            return Ok(new { FileId = fileId });
        }

        [HttpGet("download/{fileId}")]
        public async Task<IActionResult> DownloadFile(string fileId)
        {
            var (fileStream, contentType) = await _storageService.DownloadFileAsync(fileId);
            return File(fileStream, contentType);
        }

        [HttpDelete("delete/{fileId}")]
        public async Task<IActionResult> DeleteFile(string fileId)
        {
            var result = await _storageService.DeleteFileAsync(fileId);
            if (result)
            {
                return NoContent();
            }
            return NotFound();
        }
    }
}
