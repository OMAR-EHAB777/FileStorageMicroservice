using FileStorageMicroservice.Repositories;
using FileStorageMicroservice.Services;
using Microsoft.AspNetCore.Authorization;
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

        /// <summary>
        /// Uploads a file to local storage.
        /// </summary>
        /// <param name="file">The file to upload.</param>
        /// <returns>The ID of the uploaded file.</returns>
        [Authorize]
        [HttpPost("upload")]
        public async Task<IActionResult> UploadFile(IFormFile file)
        {
            var fileId = await _localFileStorageService.UploadFileAsync(file);
            return Ok(new { FileId = fileId });
        }

        /// <summary>
        /// Downloads a file from local storage.
        /// </summary>
        /// <param name="fileId">The ID of the file to download.</param>
        /// <returns>The file stream and content type.</returns>
        [Authorize]
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

        /// <summary>
        /// Deletes a file by its ID.
        /// </summary>
        /// <param name="fileId">The ID of the file to delete.</param>
        /// <returns>No content if successful, otherwise Not Found.</returns>
        [Authorize]
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
