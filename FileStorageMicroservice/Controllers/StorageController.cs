using FileStorageMicroservice.Services;
using Microsoft.AspNetCore.Authorization;
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

        /// <summary>
        /// Uploads a file to the storage.
        /// </summary>
        /// <param name="file">The file to upload.</param>
        /// <returns>The ID of the uploaded file.</returns>
        [Authorize]
        [HttpPost("upload")]
        public async Task<IActionResult> UploadFile(IFormFile file)
        {
            var fileId = await _storageService.UploadFileAsync(file);
            return Ok(new { FileId = fileId });
        }

        /// <summary>
        /// Downloads a file from the storage.
        /// </summary>
        /// <param name="fileId">The ID of the file to download.</param>
        /// <returns>The file stream and content type.</returns>
        [Authorize]
        [HttpGet("download/{fileId}")]
        public async Task<IActionResult> DownloadFile(string fileId)
        {
            var (fileStream, contentType) = await _storageService.DownloadFileAsync(fileId);
            return File(fileStream, contentType);
        }

        /// <summary>
        /// Deletes a file by its ID.
        /// </summary>
        /// <param name="fileId">The ID of the file to delete.</param>
        /// <returns>No content if successful, otherwise Not Found.</returns>
        [Authorize]
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
