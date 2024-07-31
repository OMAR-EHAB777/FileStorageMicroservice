
namespace FileStorageMicroservice.Models
{
    public class FileMetadata
    {
        public string Id { get; set; } = Guid.NewGuid().ToString(); // Example default value
        public string FileName { get; set; } = string.Empty;
        public string ContentType { get; set; } = string.Empty;
        public long Size { get; set; }
        public DateTime UploadDate { get; set; } = DateTime.UtcNow;
        public string StorageLocation { get; set; } = string.Empty;
    }

}

