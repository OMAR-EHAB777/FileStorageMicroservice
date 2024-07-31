using Microsoft.AspNetCore.Http;
using System.IO;
using System.Threading.Tasks;

namespace FileStorageMicroservice.Services
{
    public interface IStorageService
    {
        Task<string> UploadFileAsync(IFormFile file);
        Task<(Stream FileStream, string ContentType)> DownloadFileAsync(string fileId);
        Task<bool> DeleteFileAsync(string fileId);
    }
}
