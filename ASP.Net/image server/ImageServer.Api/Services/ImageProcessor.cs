using ImageServer.Api.Services.Interfaces;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;

namespace ImageServer.Api.Services;

public class ImageProcessor : IImageProcessor
{
    public async Task<string> CreateThumbnailAsync(string sourcePath, string targetDirectory, int width = 300, int height = 300)
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

        // Set JPEG quality to 80% for a good balance between quality and file size
        await image.SaveAsJpegAsync(thumbnailPath, new SixLabors.ImageSharp.Formats.Jpeg.JpegEncoder
        {
            Quality = 80
        });
        return thumbnailPath;
    }
}
