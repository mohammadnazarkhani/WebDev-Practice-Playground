namespace ImageServer.Api.Services.Interfaces;

public interface IFileService
{
    Task<(bool isValid, string? error)> ValidateFile(IFormFile? file);
    Task SaveFileAsync(IFormFile file, string filePath);
    string GetFilePath(Guid imageId, string fileName);
    void DeleteFile(string filePath);
}
