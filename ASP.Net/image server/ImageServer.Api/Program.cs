using Microsoft.EntityFrameworkCore;
using ImageServer.Api.Data;
using ImageServer.Api.Endpoints;
using Microsoft.AspNetCore.Http.Features;
using ImageServer.Api.Services;
using ImageServer.Api.Services.Interfaces;
using FluentValidation;
using ImageServer.Api.Validators;
using ImageServer.Api.Services.Settings;
using ImageServer.Api.Middleware;
using Microsoft.OpenApi.Models;
using ImageServer.Api.Swagger;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.AspNetCore.Server.IISIntegration;
using ImageServer.Api.Extensions.Middleware;
using ImageServer.Api.Models;
using ImageServer.Api.Data.UnitOfWork;

// Ensure upload directory exists before app configuration
var currentDirectory = Directory.GetCurrentDirectory();
var uploadDirectory = Path.Combine(currentDirectory, "uploads");
if (!Directory.Exists(uploadDirectory))
{
    Directory.CreateDirectory(uploadDirectory);
}

// Create wwwroot to satisfy the framework
Directory.CreateDirectory(Path.Combine(currentDirectory, "wwwroot"));

var builder = WebApplication.CreateBuilder(args);

// Configure paths and disable static files
builder.Environment.WebRootPath = string.Empty;  // Use empty string instead of null
builder.Configuration["StaticWebAssets:Enabled"] = "false";

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Image Server API", Version = "v1" });
    c.MapType<IFormFile>(() => new OpenApiSchema
    {
        Type = "string",
        Format = "binary"
    });
});

builder.Services.AddDbContext<ImageDbContext>(options =>
    options.UseInMemoryDatabase("ImageDb"));

// Add repositories and unit of work
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

builder.Services.AddAntiforgery(options =>
{
    options.FormFieldName = "__RequestVerificationToken";
    options.HeaderName = "X-CSRF-TOKEN";
    options.SuppressXFrameOptionsHeader = false;
});

builder.Services.AddHttpContextAccessor();

// Configure file size limits
builder.Services.Configure<IISServerOptions>(options =>
{
    options.MaxRequestBodySize = 104857600; // 100MB
});

builder.Services.Configure<KestrelServerOptions>(options =>
{
    options.Limits.MaxRequestBodySize = 104857600; // 100MB
});

builder.Services.Configure<FormOptions>(options =>
{
    options.ValueLengthLimit = 104857600; // 100MB
    options.MultipartBodyLengthLimit = 104857600; // 100MB
    options.MemoryBufferThreshold = 104857600; // 100MB
});

// Add memory cache
builder.Services.AddMemoryCache();

// Register file validation service
builder.Services.AddSingleton<IFileValidationService, FileValidationService>();

// Register ImageFileSettings
builder.Services.Configure<ImageFileSettings>(builder.Configuration.GetSection("FileSettings:Image"));
builder.Services.AddSingleton<IFileSettings>(sp =>
{
    var configuration = sp.GetRequiredService<IConfiguration>();
    return new ImageFileSettings(configuration);
});

// Register services
builder.Services.AddScoped<IImagePersistenceService, ImagePersistenceService>();
builder.Services.AddScoped<IFileService, FileService>();
builder.Services.AddScoped<IImageService, ImageService>();
builder.Services.AddScoped<IImageProcessor, ImageProcessor>();

// Configure CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", builder =>
    {
        builder.AllowAnyOrigin()
               .AllowAnyMethod()
               .AllowAnyHeader();
    });
});

// Add validators
builder.Services.AddValidatorsFromAssemblyContaining<FileValidator>();

// Configure cookie policy
builder.Services.Configure<CookiePolicyOptions>(options =>
{
    options.CheckConsentNeeded = context => true;
    options.MinimumSameSitePolicy = SameSiteMode.None;
});

if (builder.Environment.IsDevelopment())
{
    builder.WebHost.UseUrls("http://localhost:5000", "https://localhost:7000");
    builder.Services.AddLogging(logging =>
    {
        logging.ClearProviders();
        logging.AddConsole();
        logging.AddDebug();
    });
}

var app = builder.Build();

try
{
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI(c =>
        {
            c.SwaggerEndpoint("/swagger/v1/swagger.json", "Image Server API v1");
            c.RoutePrefix = "swagger";
        });
    }

    app.UseHttpsRedirection();
    app.UseCookiePolicy();
    app.UseAntiforgery();
    app.UseCors("AllowAll");
    app.UseFileValidation();
    app.MapImageEndpoints();

    app.Run();
}
catch (Exception ex)
{
    var logger = app.Services.GetRequiredService<ILogger<Program>>();
    logger.LogCritical(ex, "Application terminated unexpectedly");
    throw;
}
