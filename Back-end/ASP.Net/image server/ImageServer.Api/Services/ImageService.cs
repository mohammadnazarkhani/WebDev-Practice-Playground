using ImageServer.Api.Data;
using ImageServer.Api.Models;
using Microsoft.EntityFrameworkCore;
using ImageServer.Api.Services.Interfaces;
using ImageServer.Api.DTOs;
using Microsoft.AspNetCore.Http;
using ImageServer.Api.DTOs.Images.Requests;
using ImageServer.Api.DTOs.Images.Responses;
using ImageServer.Api.Mappings;
using Microsoft.Extensions.Logging;

namespace ImageServer.Api.Services;

public class ImageService : IImageService
{
    private readonly IImagePersistenceService _persistenceService;
    private readonly IFileService _fileService;
    private readonly IFileSettings _fileSettings;
    private readonly IImageProcessor _imageProcessor;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly ILogger<ImageService> _logger;

    public ImageService(
        IImagePersistenceService persistenceService,
        IFileService fileService,
        IFileSettings fileSettings,
        IImageProcessor imageProcessor,
        IHttpContextAccessor httpContextAccessor,
        ILogger<ImageService> logger)
    {
        _persistenceService = persistenceService;
        _fileService = fileService;
        _fileSettings = fileSettings;
        _imageProcessor = imageProcessor;
        _httpContextAccessor = httpContextAccessor;
        _logger = logger;
    }

    public async Task<(bool success, object result)> UploadImageAsync(ImageUploadDto uploadDto)
    {
        try
        {
            var (isValid, error) = await _fileService.ValidateFile(uploadDto.File);
            if (!isValid)
                return (false, new { error });

            var image = await _persistenceService.CreateImageAsync(uploadDto.Name, uploadDto.File);

            var filePath = _fileService.GetFilePath(image.Id, uploadDto.File.FileName);
            await _fileService.SaveFileAsync(uploadDto.File, filePath);

            image.FilePath = filePath;
            image.ThumbnailPath = await CreateThumbnailAsync(filePath);

            await _persistenceService.SaveChangesAsync();
            return (true, image.ToDto(_httpContextAccessor));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error uploading image");
            return (false, new { error = "Error uploading image" });
        }
    }

    public async Task<List<ImageResponseDto>> GetImagesAsync()
    {
        var images = await _persistenceService.GetAllImagesAsync();
        return images.Select(i => i.ToDto(_httpContextAccessor)).ToList();
    }

    public async Task<(bool found, string? filePath, string? error)> GetImageAsync(Guid id)
    {
        var image = await _persistenceService.GetImageAsync(id);
        if (image == null) return (false, null, "Image not found");

        if (!File.Exists(image.FilePath))
            return (false, null, "Image file not found");

        return (true, image.FilePath, null);
    }

    public async Task<(bool found, string? filePath, string? error)> GetThumbnailAsync(Guid id)
    {
        var image = await _persistenceService.GetImageAsync(id);
        if (image == null) return (false, null, "Image not found");

        if (string.IsNullOrEmpty(image.ThumbnailPath) || !File.Exists(image.ThumbnailPath))
        {
            // Regenerate thumbnail if missing
            if (File.Exists(image.FilePath))
            {
                image.ThumbnailPath = await CreateThumbnailAsync(image.FilePath);
                await _persistenceService.SaveChangesAsync();
            }
            else
                return (false, null, "Image file not found");
        }

        return (true, image.ThumbnailPath, null);
    }

    public async Task<(bool success, string message)> DeleteImageAsync(Guid id)
    {
        var image = await _persistenceService.GetImageAsync(id);
        if (image == null) return (false, "Image not found");

        _fileService.DeleteFile(image.FilePath);
        if (!string.IsNullOrEmpty(image.ThumbnailPath))
            _fileService.DeleteFile(image.ThumbnailPath);

        await _persistenceService.DeleteImageAsync(image);
        await _persistenceService.SaveChangesAsync();  // Add this line

        return (true, "Image deleted successfully");
    }

    public async Task<(bool success, object result)> UpdateImageAsync(Guid id, string? name, IFormFile? file)
    {
        if (file == null && string.IsNullOrWhiteSpace(name))
            return (false, new { error = "Either name or file must be provided" });

        var image = await _persistenceService.GetImageAsync(id);
        if (image == null)
            return (false, new { error = "Image not found" });

        if (file != null)
        {
            var (isValid, error) = await _fileService.ValidateFile(file);
            if (!isValid)
                return (false, new { error });

            _fileService.DeleteFile(image.FilePath);
            if (!string.IsNullOrEmpty(image.ThumbnailPath))
                _fileService.DeleteFile(image.ThumbnailPath);

            var filePath = _fileService.GetFilePath(image.Id, file.FileName);
            await _fileService.SaveFileAsync(file, filePath);

            image.FilePath = filePath;
            image.ContentType = file.ContentType;
            image.FileSize = file.Length;

            // Create new thumbnail
            image.ThumbnailPath = await CreateThumbnailAsync(filePath);
        }

        if (!string.IsNullOrWhiteSpace(name))
        {
            image.Name = name.Trim();
        }

        image.UpdatedAt = DateTime.UtcNow;
        await _persistenceService.SaveChangesAsync();

        return (true, image.ToDto(_httpContextAccessor));
    }

    public async Task<(bool success, object result)> PatchImageAsync(Guid id, string? name, IFormFile? file)
    {
        if (file == null && string.IsNullOrWhiteSpace(name))
            return (false, new { error = "Either name or file must be provided" });

        var image = await _persistenceService.GetImageAsync(id);
        if (image == null)
            return (false, new { error = "Image not found" });

        if (file != null)
        {
            var (isValid, error) = await _fileService.ValidateFile(file);
            if (!isValid)
                return (false, new { error });

            _fileService.DeleteFile(image.FilePath);
            if (!string.IsNullOrEmpty(image.ThumbnailPath))
                _fileService.DeleteFile(image.ThumbnailPath);

            var filePath = _fileService.GetFilePath(image.Id, file.FileName);
            await _fileService.SaveFileAsync(file, filePath);

            image.FilePath = filePath;
            image.ContentType = file.ContentType;
            image.FileSize = file.Length;

            // Create new thumbnail
            image.ThumbnailPath = await CreateThumbnailAsync(filePath);
        }

        if (!string.IsNullOrWhiteSpace(name))
        {
            image.Name = name.Trim();
        }

        image.UpdatedAt = DateTime.UtcNow;
        await _persistenceService.SaveChangesAsync();

        return (true, image.ToDto(_httpContextAccessor));
    }

    public (bool success, string message) CleanupUploads()
    {
        var uploadsPath = Path.Combine(Directory.GetCurrentDirectory(), _fileSettings.UploadPath);
        if (!Directory.Exists(uploadsPath))
        {
            return (false, $"Directory '{_fileSettings.UploadPath}' does not exist.");
        }

        try
        {
            foreach (var file in Directory.GetFiles(uploadsPath))
            {
                _fileService.DeleteFile(file);
            }
            return (true, $"Directory '{_fileSettings.UploadPath}' cleaned successfully.");
        }
        catch (Exception ex)
        {
            return (false, $"Failed to clean directory '{_fileSettings.UploadPath}': {ex.Message}");
        }
    }

    private async Task<string> CreateThumbnailAsync(string sourcePath)
    {
        var uploadsPath = Path.Combine(Directory.GetCurrentDirectory(), _fileSettings.UploadPath);
        return await _imageProcessor.CreateThumbnailAsync(sourcePath, uploadsPath);
    }
}
