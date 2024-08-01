using System.IO;
using System.Threading.Tasks;
using FileStorageMicroservice.Configurations;
using FileStorageMicroservice.Models;
using FileStorageMicroservice.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Moq;
using Xunit;

public class LocalFileStorageServiceTests
{

    private readonly string _testDirectory;
    private readonly Mock<IFileMetadataRepository> _mockMetadataRepository;
    private readonly LocalFileStorageService _localFileStorageService;

    public LocalFileStorageServiceTests()
    {
        // Setup a temporary directory for testing
        _testDirectory = Path.Combine(Path.GetTempPath(), "LocalFileStorageTests");

        // Create a mock configuration
        var inMemorySettings = new Dictionary<string, string>
        {
            {"LocalStorage:Directory", _testDirectory}
        };

        IConfiguration configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(inMemorySettings)
            .Build();

        // Mock the IFileMetadataRepository
        _mockMetadataRepository = new Mock<IFileMetadataRepository>();

        // Initialize the LocalFileStorageService with the configuration and mock
        _localFileStorageService = new LocalFileStorageService(configuration, _mockMetadataRepository.Object);

        // Ensure the test directory exists
        Directory.CreateDirectory(_testDirectory);
    }

    [Fact]
    public async Task UploadFileAsync_ShouldSaveFile()
    {
        // Arrange
        var fileMock = new Mock<IFormFile>();
        var content = "Hello World from a fake file";
        var fileName = "test.txt";
        var ms = new MemoryStream();
        var writer = new StreamWriter(ms);
        writer.Write(content);
        writer.Flush();
        ms.Position = 0;

        fileMock.Setup(_ => _.OpenReadStream()).Returns(ms);
        fileMock.Setup(_ => _.FileName).Returns(fileName);
        fileMock.Setup(_ => _.Length).Returns(ms.Length);

        _mockMetadataRepository.Setup(repo => repo.SaveFileMetadataAsync(It.IsAny<FileMetadata>()))
                               .Returns(Task.CompletedTask);

        // Act
        var fileId = await _localFileStorageService.UploadFileAsync(fileMock.Object);

        // Assert
        _mockMetadataRepository.Verify(repo => repo.SaveFileMetadataAsync(It.IsAny<FileMetadata>()), Times.Once);
        Assert.False(string.IsNullOrEmpty(fileId));
    }


    [Fact]
    public async Task DownloadFileAsync_ShouldReturnFileStream()
    {
        // Arrange
        var fileId = Guid.NewGuid().ToString();
        var fileName = "test.txt";
        var filePath = Path.Combine(_testDirectory, fileName);
        var fileContent = "Test content";

        await File.WriteAllTextAsync(filePath, fileContent);

        var fileMetadata = new FileMetadata
        {
            Id = fileId,
            FileName = fileName,
            ContentType = "text/plain",
            Size = fileContent.Length,
            UploadDate = DateTime.UtcNow,
            StorageLocation = filePath
        };

        _mockMetadataRepository.Setup(repo => repo.GetFileMetadataAsync(fileId))
                               .ReturnsAsync(fileMetadata);

        // Act
        var result = await _localFileStorageService.DownloadFileAsync(fileId);

        // Assert
        Assert.NotNull(result.FileStream);
        Assert.Equal("text/plain", result.ContentType);

        // Clean up
        result.FileStream.Dispose();
    }


    [Fact]
    public async Task DeleteFileAsync_ShouldDeleteFile()
    {
        // Arrange
        var fileId = Guid.NewGuid().ToString();
        var fileName = "test.txt";
        var filePath = Path.Combine(_testDirectory, fileName);
        var fileContent = "Test content";

        await File.WriteAllTextAsync(filePath, fileContent);

        var fileMetadata = new FileMetadata
        {
            Id = fileId,
            FileName = fileName,
            ContentType = "text/plain",
            Size = fileContent.Length,
            UploadDate = DateTime.UtcNow,
            StorageLocation = filePath
        };

        _mockMetadataRepository.Setup(repo => repo.GetFileMetadataAsync(fileId))
                               .ReturnsAsync(fileMetadata);
        _mockMetadataRepository.Setup(repo => repo.DeleteFileMetadataAsync(fileId))
                               .Returns(Task.CompletedTask);

        // Act
        var result = await _localFileStorageService.DeleteFileAsync(fileId);

        // Assert
        Assert.True(result);
        _mockMetadataRepository.Verify(repo => repo.DeleteFileMetadataAsync(fileId), Times.Once);
        Assert.False(File.Exists(filePath));
    }

}
