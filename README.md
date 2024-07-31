# FileStorageMicroservice

A microservice for managing file uploads, downloads, and deletions, using ASP.NET Core and AWS S3. The service can also be configured to use a local file system for storage.

## Features

- Upload files of any type
- Download files using unique identifiers
- Delete files from the storage
- Supports AWS S3 and local file system storage

## Prerequisites

- [.NET 7.0 SDK](https://dotnet.microsoft.com/download/dotnet/7.0)
- [AWS Account](https://aws.amazon.com/) (if using AWS S3)
- [MinIO](https://min.io/) (optional, for local S3-compatible storage)

## Configuration

### AWS S3 Setup

To use AWS S3 for storage, update the `appsettings.json` with your AWS credentials and bucket details:

```json
"AWS": {
  "Region": "us-west-2",
  "BucketName": "your-bucket-name",
  "AccessKey": "your-access-key",
  "SecretKey": "your-secret-key"
}
```

### Local File System Setup

To use the local file system for storage, update `appsettings.json`:

```json
"LocalStorage": {
  "Directory": "C:\\path\\to\\storage"
}
```

## Running the Application

1. **Clone the repository**:

   ```bash
   git clone https://github.com//OMAR-EHAB777/FileStorageMicroservice.git
   cd FileStorageMicroservice
   ```

2. **Install dependencies**:

   ```bash
   dotnet restore
   ```

3. **Run the application**:

   ```bash
   dotnet run
   ```

4. **Access Swagger UI**:

   The API documentation is available at [http://localhost:5000/swagger](http://localhost:5000/swagger).

## API Endpoints

- **POST /api/storage/upload**: Upload a file.
- **GET /api/storage/download/{fileId}**: Download a file by ID.
- **DELETE /api/storage/delete/{fileId}**: Delete a file by ID.

## Testing

### Unit Tests

- Ensure the project has a test suite set up with appropriate unit tests for services and controllers.

## Deployment

- Ensure the application is properly configured for production use, including securing API keys and configuring HTTPS.

## License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.
