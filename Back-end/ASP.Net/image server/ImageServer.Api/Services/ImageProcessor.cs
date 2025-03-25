using ImageServer.Api.Services.Interfaces;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;

namespace ImageServer.Api.Services;

public class ImageProcessor : IImageProcessor
{
    public async Task<string> CreateThumbnailAsync(string sourcePath, string targetDirectory, int width = 150, int height = 150)
    {
        var fileName = Path.GetFileName(sourcePath);
        var thumbnailFileName = $"thumb_{fileName}";
        var thumbnailPath = Path.Combine(targetDirectory, thumbnailFileName);

        using var image = await Image.LoadAsync(sourcePath);

        // Calculate dimensions maintaining aspect ratio
        var ratio = Math.Min((double)width / image.Width, (double)height / image.Height);
        var newWidth = (int)(image.Width * ratio);
        var newHeight = (int)(image.Height * ratio);

        image.Mutate(x => x
            .Resize(newWidth, newHeight)
            .BackgroundColor(Color.White)); // Optional: add background color for transparent images

        await image.SaveAsync(thumbnailPath);
        return thumbnailPath;
    }
}
