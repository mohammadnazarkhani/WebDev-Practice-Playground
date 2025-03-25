using ImageServer.Api.Models;

namespace ImageServer.Api.Services.Interfaces;

public interface IImagePersistenceService
{
    Task<Image> CreateImageAsync(string name, IFormFile file);
    Task<Image?> GetImageAsync(Guid id);
    Task<List<Image>> GetAllImagesAsync();
    Task<Image> UpdateImageAsync(Image image);
    Task DeleteImageAsync(Image image);
    Task SaveChangesAsync();
}
