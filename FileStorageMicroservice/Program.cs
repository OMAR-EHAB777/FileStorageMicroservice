using FileStorageMicroservice.Configurations;
using FileStorageMicroservice.Data;
using FileStorageMicroservice.Repositories;
using FileStorageMicroservice.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "File Storage Microservice API",
        Version = "v1",
        Description = "API documentation for the File Storage Microservice",
        Contact = new OpenApiContact
        {
            Name = "Omar Ehab",
            Email = "omarehabdev@gmail.com",
            Url = new Uri("https://gravatar.com/omarehabdev")
        }
    });

    // Optionally, include XML comments if you have added them to your methods
    var xmlFile = $"{System.Reflection.Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    c.IncludeXmlComments(xmlPath);
});

// Register the DbContext
builder.Services.AddDbContext<StorageDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));


// Register configuration and services
builder.Services.AddScoped<IFileMetadataRepository, FileMetadataRepository>();
builder.Services.AddScoped<IStorageService, S3StorageService>();
builder.Services.AddScoped<IStorageService>(provider =>
{
    var configuration = provider.GetRequiredService<IConfiguration>();
    var metadataRepository = provider.GetRequiredService<IFileMetadataRepository>();
    return new LocalFileStorageService(configuration, metadataRepository);
});


// Build the application
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "File Storage Microservice API v1");
    });
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();
