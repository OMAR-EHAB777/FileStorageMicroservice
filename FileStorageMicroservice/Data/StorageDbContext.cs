using Microsoft.EntityFrameworkCore;
using FileStorageMicroservice.Models;
using System.Collections.Generic;

namespace FileStorageMicroservice.Data
{
    public class StorageDbContext : DbContext
    {
        public StorageDbContext(DbContextOptions<StorageDbContext> options) : base(options) { }

        public DbSet<FileMetadata> FileMetadatas { get; set; }
    }
}
