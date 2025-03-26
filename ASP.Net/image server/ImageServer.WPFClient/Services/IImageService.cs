using System;
using System.IO;
using ImageServer.WPFClient.Models;

namespace ImageServer.WPFClient.Services;

public interface IImageService
{
    Task<List<ImageModel>> GetImagesAsync();
    Task<ImageModel> UploadImageAsync(string name, Stream imageStream, string fileName);
    Task<bool> DeleteImageAsync(Guid id);
    Task<bool> DeleteAllImagesAsync();
    Task<ImageModel> UpdateImageAsync(Guid id, string name, Stream? imageStream = null);
}
