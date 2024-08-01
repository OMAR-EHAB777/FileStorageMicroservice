
# FileStorageMicroservice

A microservice for managing file uploads, downloads, and deletions, using ASP.NET Core and AWS S3. The service can also be configured to use a local file system for storage.

## Features

- Upload files of any type
- Download files using unique identifiers
- Delete files from the storage
- Supports AWS S3 and local file system storage

## Functional and Non-Functional Requirements

### Functional Requirements
1. **File Upload**: Allows other services to upload files of any extension.
2. **File Retrieval**: Provides a way to retrieve files using unique identifiers.
3. **File Deletion**: Supports the deletion of files.
4. **File Metadata Storage**: Stores and retrieves metadata such as file size, type, upload date, etc.
5. **Access Control**: Ensures only authorized services can access storage functionalities.

### Non-Functional Requirements
1. **Scalability**: Can scale to accommodate a growing number of files and requests.
2. **Performance**: Optimized for fast upload and download speeds.
3. **Security**: Implements encryption for data both in transit and at rest.
4. **Availability**: Designed for high availability and minimal downtime.
5. **Durability**: Ensures data integrity and durability.
6. **Logging and Monitoring**: Maintains detailed logs and monitoring for access and errors.

## High-Level Design

### Diagram

![High-Level Design](![FileStorageMicroservice drawio](https://github.com/user-attachments/assets/dc633d81-131c-44fe-8e07-f6dc29483bdf))


### Components
1. **API Gateway**: Manages and routes incoming requests.
2. **File Storage Microservice**:
   - **API Layer**: Handles RESTful API requests.
   - **Storage Manager**: Implements business logic for file operations.
   - **Storage Adapter**: Interfaces with storage backends like AWS S3 or local file system.
   - **Metadata Store**: Manages file metadata.

3. **Storage Backend**: The storage system (AWS S3, local file system, etc.)

## Communication with Other Microservices

### REST API
Other microservices communicate with the storage microservice via RESTful APIs. This includes operations for file upload, retrieval, and deletion.

#### Endpoints
- **POST /api/storage/upload**: Upload a file.
- **GET /api/storage/download/{fileId}**: Download a file by ID.
- **DELETE /api/storage/delete/{fileId}**: Delete a file by ID.

### Security and Access Control
JWT Authentication: The service uses JSON Web Tokens (JWT) for secure access. Other services must include a valid JWT token in the Authorization header for each request.
Token Format: Bearer {token}
Token Issuance: Tokens are issued by an Identity Provider (IdP) within the organization, and all services must validate the tokens' authenticity.
### Asynchronous Communication
1. Message Queue

    Queue System: RabbitMQ or similar message broker.
    Purpose: For non-blocking file operations, such as processing large uploads or notifying other services of changes.
    Example Usage:
        Upload Notification: When a file is uploaded, a message is sent to a queue to notify other services.
        Deletion Notification: Similarly, a message is sent when a file is deleted.

2. Message Format

    Example Message:

    json

    {
      "event": "FileUploaded",
      "fileId": "12345",
      "fileName": "example.txt",
      "uploaderService": "ServiceName",
      "timestamp": "2024-08-01T14:30:00Z"
    }

3. Implementation Considerations

    Durability: Messages should be durable and persist even if the message broker temporarily goes down.
    Security: Secure the message channel using TLS and authenticate services using credentials or certificates.

Error Handling

1. HTTP Response Codes

    200 OK: Successful operation.
    400 Bad Request: Invalid input or request format.
    401 Unauthorized: Missing or invalid authentication token.
    404 Not Found: File not found.
    500 Internal Server Error: An unexpected error occurred.

2. Logging and Monitoring

    All API requests and responses are logged.
    Errors are monitored and alerts are configured for critical issues.
## Prerequisites

- [.NET 7.0 SDK](https://dotnet.microsoft.com/download/dotnet/7.0)
- [AWS Account](https://aws.amazon.com/) (if using AWS S3)

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
   git clone https://github.com/OMAR-EHAB777/FileStorageMicroservice.git
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
Ensure the project has a test suite set up with appropriate unit tests for services and controllers.
```bash
   dotnet test
   ```
## License
This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.

