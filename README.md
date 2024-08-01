
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

![High-Level Design](![FileStorageMicroservice drawio](https://github.com/user-attachments/assets/dc633d81-131c-44fe-8e07-f6dc29483bdf)
)

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

### Asynchronous Communication
For non-blocking operations, a message queue (e.g., RabbitMQ) can be used to process file uploads asynchronously.

### Security and Access Control
OAuth2 or JWT tokens are used to secure communication, ensuring that only authenticated and authorized requests are processed.

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

