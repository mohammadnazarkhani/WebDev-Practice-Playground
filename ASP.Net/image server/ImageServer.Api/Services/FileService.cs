using Microsoft.Extensions.Options;
using ImageServer.Api.Services.Interfaces;
using FluentValidation;
using Microsoft.AspNetCore.Http;

namespace ImageServer.Api.Services;

public class FileService : IFileService
{
    private readonly IFileSettings _settings;
    private readonly IValidator<IFormFile> _validator;

    public FileService(IFileSettings settings, IValidator<IFormFile> validator)
    {
        _settings = settings;
        _validator = validator;
    }

    public async Task<(bool isValid, string? error)> ValidateFile(IFormFile? file)
    {
        if (file == null)
            return (false, "No file provided");

        var validationResult = await _validator.ValidateAsync(file);
        if (!validationResult.IsValid)
            return (false, validationResult.Errors.First().ErrorMessage);

        return (true, null);
    }

    public async Task SaveFileAsync(IFormFile file, string filePath)
    {
        var directory = Path.GetDirectoryName(filePath);
        if (!Directory.Exists(directory))
            Directory.CreateDirectory(directory!);

        using var stream = new FileStream(filePath, FileMode.Create);
        await file.CopyToAsync(stream);
    }

    public string GetFilePath(Guid imageId, string fileName)
    {
        var extension = Path.GetExtension(fileName).ToLower();
        var uploadsPath = Path.Combine(Directory.GetCurrentDirectory(), _settings.UploadPath);
        return Path.Combine(uploadsPath, $"{imageId}{extension}");
    }

    public void DeleteFile(string filePath)
    {
        if (File.Exists(filePath))
            File.Delete(filePath);
    }
}
