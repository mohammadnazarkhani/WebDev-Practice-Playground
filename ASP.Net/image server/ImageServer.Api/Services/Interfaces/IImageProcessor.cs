namespace ImageServer.Api.Services.Interfaces;

public interface IImageProcessor
{
    Task<string> CreateThumbnailAsync(string sourcePath, string targetDirectory, int width = 150, int height = 150);
}
