namespace ImageServer.Api.Services.Interfaces;

public interface IFileValidationService
{
    (bool isValid, IEnumerable<string> errors) ValidateFiles(IFormFileCollection files);
    bool IsFileValid(IFormFile file, IFileSettings settings);
    bool IsContentTypeValid(string contentType, IFileSettings settings);
}
