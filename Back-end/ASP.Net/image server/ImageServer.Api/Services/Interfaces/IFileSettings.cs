namespace ImageServer.Api.Services.Interfaces;

public interface IFileSettings
{
    long MaxFileSize { get; }
    string[] AllowedExtensions { get; }
    string UploadPath { get; }
    string FileType { get; }
}
