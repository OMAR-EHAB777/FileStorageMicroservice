# FileStorageMicroservice

## Overview

The FileStorageMicroservice is a scalable, cloud-based microservice built with ASP.NET Core for handling file storage operations. It supports uploading, downloading, and deleting files of any type, and stores metadata for efficient file management. The service uses AWS S3 for storage and PostgreSQL for metadata.

## Features

- **Upload Files**: Allows uploading files of any type.
- **Download Files**: Retrieve files using a unique identifier.
- **Delete Files**: Remove files by ID.
- **File Metadata Management**: Store and retrieve metadata like file size, type, and upload date.
- **Secure and Scalable**: Implemented with security best practices and designed for scalability.

## Getting Started

### Prerequisites

- [.NET 7 SDK](https://dotnet.microsoft.com/download/dotnet/7.0)
- [Visual Studio 2022](https://visualstudio.microsoft.com/vs/)
- [AWS Account](https://aws.amazon.com/) (for S3 storage)
- [PostgreSQL](https://www.postgresql.org/) (for metadata storage)

### Setup

1. Clone the repository:

   ```bash
   git clone https://github.com/your-username/FileStorageMicroservice.git
   cd FileStorageMicroservice
