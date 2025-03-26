using ImageServer.Api.Models;
using ImageServer.Api.Services.Interfaces;
using ImageServer.Api.Data.UnitOfWork;

namespace ImageServer.Api.Services;

public class ImagePersistenceService : IImagePersistenceService
{
    private readonly IUnitOfWork _unitOfWork;

    public ImagePersistenceService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Image> CreateImageAsync(string name, IFormFile file)
    {
        var image = new Image
        {
            Id = Guid.NewGuid(),
            Name = name.Trim(),
            ContentType = file.ContentType,
            FileSize = file.Length,
            CreatedAt = DateTime.UtcNow
        };

        await _unitOfWork.Images.AddAsync(image);
        return image;
    }

    public async Task<Image?> GetImageAsync(Guid id) =>
        await _unitOfWork.Images.GetByIdAsync(id);

    public async Task<List<Image>> GetAllImagesAsync() =>
        (await _unitOfWork.Images.GetAllAsync()).ToList();

    public Task<Image> UpdateImageAsync(Image image)
    {
        image.UpdatedAt = DateTime.UtcNow;
        _unitOfWork.Images.Update(image);
        return Task.FromResult(image);
    }

    public Task DeleteImageAsync(Image image)
    {
        _unitOfWork.Images.Remove(image);
        return Task.CompletedTask;
    }

    public async Task DeleteAllImagesAsync()
    {
        var images = await _unitOfWork.Images.GetAllAsync();
        foreach (var image in images)
        {
            _unitOfWork.Images.Remove(image);
        }
    }

    public async Task SaveChangesAsync() =>
        await _unitOfWork.SaveChangesAsync();
}
