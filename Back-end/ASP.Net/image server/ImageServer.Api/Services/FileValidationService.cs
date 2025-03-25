using ImageServer.Api.Services.Interfaces;
using Microsoft.Extensions.Caching.Memory;

namespace ImageServer.Api.Services;

public class FileValidationService : IFileValidationService
{
    private readonly IEnumerable<IFileSettings> _fileSettings;
    private readonly IMemoryCache _cache;
    private readonly ILogger<FileValidationService> _logger;

    public FileValidationService(
        IEnumerable<IFileSettings> fileSettings,
        IMemoryCache cache,
        ILogger<FileValidationService> logger)
    {
        _fileSettings = fileSettings;
        _cache = cache;
        _logger = logger;
    }

    public (bool isValid, IEnumerable<string> errors) ValidateFiles(IFormFileCollection files)
    {
        var errors = new List<string>();

        foreach (var file in files)
        {
            var cacheKey = $"file_validation_{file.FileName}_{file.Length}";

            if (!_cache.TryGetValue(cacheKey, out bool isValid))
            {
                isValid = _fileSettings.Any(settings => IsFileValid(file, settings));

                var cacheOptions = new MemoryCacheEntryOptions()
                    .SetSlidingExpiration(TimeSpan.FromMinutes(10));

                _cache.Set(cacheKey, isValid, cacheOptions);
            }

            if (!isValid)
            {
                errors.Add($"File {file.FileName} does not meet the configured requirements");
                _logger.LogWarning("File validation failed for {FileName}", file.FileName);
            }
        }

        return (errors.Count == 0, errors);
    }

    public bool IsFileValid(IFormFile file, IFileSettings settings)
    {
        try
        {
            if (file.Length > settings.MaxFileSize)
            {
                _logger.LogWarning("File {FileName} exceeds maximum size of {MaxSize}",
                    file.FileName, settings.MaxFileSize);
                return false;
            }

            var extension = Path.GetExtension(file.FileName).ToLower();
            if (!settings.AllowedExtensions.Contains(extension))
            {
                _logger.LogWarning("File {FileName} has invalid extension {Extension}",
                    file.FileName, extension);
                return false;
            }

            if (!IsContentTypeValid(file.ContentType, settings))
            {
                _logger.LogWarning("File {FileName} has invalid content type {ContentType}",
                    file.FileName, file.ContentType);
                return false;
            }

            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error validating file {FileName}", file.FileName);
            return false;
        }
    }

    public bool IsContentTypeValid(string contentType, IFileSettings settings)
    {
        return settings.FileType switch
        {
            "image" => contentType.StartsWith("image/", StringComparison.OrdinalIgnoreCase),
            // Add more file types as needed
            _ => false
        };
    }
}
