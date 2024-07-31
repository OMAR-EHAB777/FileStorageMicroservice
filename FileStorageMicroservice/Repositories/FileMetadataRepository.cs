using FileStorageMicroservice.Data;
using FileStorageMicroservice.Models;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace FileStorageMicroservice.Repositories
{
    public interface IFileMetadataRepository
    {
        Task<FileMetadata> GetFileMetadataAsync(string id);
        Task SaveFileMetadataAsync(FileMetadata fileMetadata);
        Task DeleteFileMetadataAsync(string id);
    }

    public class FileMetadataRepository : IFileMetadataRepository
    {
        private readonly StorageDbContext _context;

        public FileMetadataRepository(StorageDbContext context)
        {
            _context = context;
        }

        public async Task<FileMetadata> GetFileMetadataAsync(string id)
        {
            var fileMetadata = await _context.FileMetadatas.FindAsync(id);
            if (fileMetadata == null)
            {
                throw new KeyNotFoundException($"FileMetadata with ID '{id}' not found.");
            }
            return fileMetadata;
        }


        public async Task SaveFileMetadataAsync(FileMetadata fileMetadata)
        {
            _context.FileMetadatas.Add(fileMetadata);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteFileMetadataAsync(string id)
        {
            var metadata = await _context.FileMetadatas.FindAsync(id);
            if (metadata != null)
            {
                _context.FileMetadatas.Remove(metadata);
                await _context.SaveChangesAsync();
            }
        }
    }
}
