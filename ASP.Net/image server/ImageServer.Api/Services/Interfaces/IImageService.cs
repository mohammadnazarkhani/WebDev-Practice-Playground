using ImageServer.Api.DTOs.Images.Requests;
using ImageServer.Api.DTOs.Images.Responses;

namespace ImageServer.Api.Services.Interfaces;

public interface IImageService
{
    Task<(bool success, object result)> UploadImageAsync(ImageUploadDto uploadDto);
    Task<List<ImageResponseDto>> GetImagesAsync();
    Task<(bool found, string? filePath, string? error)> GetImageAsync(Guid id);
    Task<(bool success, string message)> DeleteImageAsync(Guid id);
    Task<(bool success, object result)> UpdateImageAsync(Guid id, string? name, IFormFile? file);
    Task<(bool success, object result)> PatchImageAsync(Guid id, string? name, IFormFile? file);
    (bool success, string message) CleanupUploads();
    Task<(bool found, string? filePath, string? error)> GetThumbnailAsync(Guid id);
}
