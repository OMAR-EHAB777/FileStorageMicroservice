using FileStorageMicroservice.Data;
using FileStorageMicroservice.Repositories;
using FileStorageMicroservice.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

// JWT Authentication Configuration
var key = Encoding.ASCII.GetBytes("w+I5QZ9SxL1Kv2EgAaM1P1+Vg3W2yH2pK7FmrLup5QA="); 
builder.Services.AddAuthentication(x =>
{
    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(x =>
{
    x.RequireHttpsMetadata = false;
    x.SaveToken = true;
    x.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(key),
        ValidateIssuer = false,
        ValidateAudience = false
    };
});

// Swagger configuration (unchanged)
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
    // Add JWT token authorization
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Please enter JWT with Bearer into field",
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] { }
        }
    });

    var xmlFile = $"{System.Reflection.Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    c.IncludeXmlComments(xmlPath);
});

// Register the DbContext (unchanged)
builder.Services.AddDbContext<StorageDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// Register configuration and services (unchanged)
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
app.UseAuthentication(); // Add authentication middleware
app.UseAuthorization();

app.MapControllers();

app.Run();
