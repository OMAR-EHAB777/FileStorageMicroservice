using Amazon.S3;
using Amazon.S3.Model;
using FileStorageMicroservice.Models;
using FileStorageMicroservice.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using System;
using System.IO;
using System.Threading.Tasks;

namespace FileStorageMicroservice.Services
{
    public class S3StorageService : IStorageService
    {
        private readonly AmazonS3Client _s3Client;
        private readonly string _bucketName;
        private readonly IFileMetadataRepository _metadataRepository;

        public S3StorageService(IOptions<AwsSettings> awsSettings, IFileMetadataRepository metadataRepository)
        {
            _bucketName = awsSettings.Value.BucketName;
            _s3Client = new AmazonS3Client(awsSettings.Value.AccessKey, awsSettings.Value.SecretKey, Amazon.RegionEndpoint.GetBySystemName(awsSettings.Value.Region));
            _metadataRepository = metadataRepository;
        }

        public async Task<string> UploadFileAsync(IFormFile file)
        {
            var fileKey = Guid.NewGuid().ToString();
            using (var stream = new MemoryStream())
            {
                await file.CopyToAsync(stream);
                var request = new PutObjectRequest
                {
                    BucketName = _bucketName,
                    Key = fileKey,
                    InputStream = stream,
                    ContentType = file.ContentType
                };
                await _s3Client.PutObjectAsync(request);
            }

            var fileMetadata = new FileMetadata
            {
                Id = fileKey,
                FileName = file.FileName,
                ContentType = file.ContentType,
                Size = file.Length,
                UploadDate = DateTime.UtcNow,
                StorageLocation = $"s3://{_bucketName}/{fileKey}"
            };

            await _metadataRepository.SaveFileMetadataAsync(fileMetadata);
            return fileKey;
        }

        public async Task<(Stream FileStream, string ContentType)> DownloadFileAsync(string fileId)
        {
            var metadata = await _metadataRepository.GetFileMetadataAsync(fileId);
            if (metadata == null)
            {
                throw new FileNotFoundException("File not found.");
            }

            var request = new GetObjectRequest
            {
                BucketName = _bucketName,
                Key = fileId
            };
            var response = await _s3Client.GetObjectAsync(request);
            return (response.ResponseStream, response.Headers.ContentType);
        }

        public async Task<bool> DeleteFileAsync(string fileId)
        {
            var metadata = await _metadataRepository.GetFileMetadataAsync(fileId);
            if (metadata == null)
            {
                return false;
            }

            var request = new DeleteObjectRequest
            {
                BucketName = _bucketName,
                Key = fileId
            };
            var response = await _s3Client.DeleteObjectAsync(request);
            await _metadataRepository.DeleteFileMetadataAsync(fileId);
            return response.HttpStatusCode == System.Net.HttpStatusCode.NoContent;
        }
    }
}
