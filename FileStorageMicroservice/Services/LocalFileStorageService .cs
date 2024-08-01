using FileStorageMicroservice.Configurations;
using FileStorageMicroservice.Models;
using FileStorageMicroservice.Repositories;
using FileStorageMicroservice.Services;
using Microsoft.Extensions.Options;

public class LocalFileStorageService : IStorageService
{
    private readonly string _storageDirectory;
    private readonly IFileMetadataRepository _metadataRepository;

    public LocalFileStorageService(IConfiguration configuration, IFileMetadataRepository metadataRepository)
    {
        _storageDirectory = configuration.GetValue<string>("LocalStorage:Directory")
                            ?? throw new ArgumentNullException(nameof(_storageDirectory));
        _metadataRepository = metadataRepository ?? throw new ArgumentNullException(nameof(metadataRepository));

        if (!Directory.Exists(_storageDirectory))
        {
            Directory.CreateDirectory(_storageDirectory);
        }
    }

    public async Task<string> UploadFileAsync(IFormFile file)
    {
        // Generate a unique file name with the original extension
        var fileId = Guid.NewGuid().ToString();
        var fileExtension = Path.GetExtension(file.FileName);
        var fileName = $"{fileId}{fileExtension}";

        var filePath = Path.Combine(_storageDirectory, fileName);

        using (var fileStream = new FileStream(filePath, FileMode.Create, FileAccess.Write, FileShare.None, 4096, useAsync: true))
        {
            await file.CopyToAsync(fileStream);
        }

        var fileMetadata = new FileMetadata
        {
            Id = fileId,
            FileName = file.FileName, // Save the original file name
            ContentType = file.ContentType,
            Size = file.Length,
            UploadDate = DateTime.UtcNow,
            StorageLocation = filePath
        };

        await _metadataRepository.SaveFileMetadataAsync(fileMetadata);
        return fileId;
    }


    public async Task<(Stream FileStream, string ContentType)> DownloadFileAsync(string fileId)
    {
        var metadata = await _metadataRepository.GetFileMetadataAsync(fileId);
        if (metadata == null)
        {
            throw new FileNotFoundException("File not found.");
        }

        var fileStream = new FileStream(metadata.StorageLocation, FileMode.Open, FileAccess.Read, FileShare.Read, bufferSize: 4096, useAsync: true);
        var contentType = metadata.ContentType;

        return (fileStream, contentType);
    }

    public async Task<bool> DeleteFileAsync(string fileId)
    {
        var metadata = await _metadataRepository.GetFileMetadataAsync(fileId);
        if (metadata == null)
        {
            return false;
        }

        if (File.Exists(metadata.StorageLocation))
        {
            File.Delete(metadata.StorageLocation);
            await _metadataRepository.DeleteFileMetadataAsync(fileId);
            return true;
        }

        return false;
    }
}

