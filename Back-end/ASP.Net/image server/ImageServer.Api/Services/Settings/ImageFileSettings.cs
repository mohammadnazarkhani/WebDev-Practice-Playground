using ImageServer.Api.Services.Interfaces;
using Microsoft.Extensions.Configuration;

namespace ImageServer.Api.Services.Settings;

public class ImageFileSettings : IFileSettings
{
    public ImageFileSettings(IConfiguration configuration)
    {
        var section = configuration.GetSection("FileSettings:Image");
        MaxFileSize = section.GetValue<long>("MaxFileSize");
        AllowedExtensions = section.GetValue<string[]>("AllowedExtensions") ??
            new[] { ".jpg", ".jpeg", ".png" };
        UploadPath = section.GetValue<string>("UploadPath") ?? "uploads";

        // Ensure values are set even if configuration is missing
        if (MaxFileSize <= 0) MaxFileSize = 10 * 1024 * 1024; // 10MB default
    }

    public long MaxFileSize { get; }
    public string[] AllowedExtensions { get; }
    public string UploadPath { get; }
    public string FileType => "image";
}
