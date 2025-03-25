using FluentValidation;
using ImageServer.Api.Services.Interfaces;

namespace ImageServer.Api.Validators;

public class FileValidator : AbstractValidator<IFormFile>
{
    public FileValidator(IFileSettings settings)
    {
        RuleFor(x => x.Length)
            .NotEmpty()
            .LessThanOrEqualTo(settings.MaxFileSize)
            .WithMessage($"File size must not exceed {settings.MaxFileSize / 1024 / 1024}MB");

        RuleFor(x => Path.GetExtension(x.FileName).ToLower())
            .Must(ext => settings.AllowedExtensions.Contains(ext))
            .WithMessage($"Only {string.Join(", ", settings.AllowedExtensions)} files are allowed");
    }
}
