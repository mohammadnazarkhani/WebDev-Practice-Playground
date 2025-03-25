using FluentValidation;
using ImageServer.Api.Models;

namespace ImageServer.Api.Validators;

public class ImageValidator : AbstractValidator<Image>
{
    public ImageValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .MaximumLength(255);

        RuleFor(x => x.FilePath)
            .NotEmpty()
            .Must(path => File.Exists(path))
            .WithMessage("Image file must exist on disk");

        RuleFor(x => x.ContentType)
            .NotEmpty()
            .Must(ct => ct.StartsWith("image/"))
            .WithMessage("File must be an image");

        RuleFor(x => x.FileSize)
            .GreaterThan(0)
            .WithMessage("File size must be greater than 0");
    }
}
