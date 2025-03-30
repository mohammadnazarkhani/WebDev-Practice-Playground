using FluentValidation;
using EFCoreShowcase.Entities;

namespace EFCoreShowcase.Common.Extensions;

public static class ValidationExtensions
{
    public static IRuleBuilderOptions<T, string> MustBeValidEmail<T>(
        this IRuleBuilder<T, string> ruleBuilder)
    {
        return ruleBuilder
            .NotEmpty()
            .EmailAddress()
            .WithMessage("Invalid email format")
            .MaximumLength(256);
    }

    public static IRuleBuilderOptions<T, string> MustBeValidPhone<T>(
        this IRuleBuilder<T, string> ruleBuilder)
    {
        return ruleBuilder
            .NotEmpty()
            .Matches(@"^\+?[1-9]\d{1,14}$")
            .WithMessage("Invalid phone number format. Must follow E.164 format");
    }

    public static IRuleBuilderOptions<T, decimal> MustBePositiveMoney<T>(
        this IRuleBuilder<T, decimal> ruleBuilder)
    {
        return ruleBuilder
            .GreaterThanOrEqualTo(0)
            .WithMessage("Amount must be greater than or equal to 0");
    }

    public static IRuleBuilderOptions<T, long> MustExist<T>(this IRuleBuilder<T, long> ruleBuilder)
    {
        return ruleBuilder
            .NotEmpty()
            .GreaterThan(0)
            .WithMessage("Invalid ID provided");
    }

    public static IRuleBuilderOptions<T, string> MustBeUnique<T>(this IRuleBuilder<T, string> ruleBuilder)
    {
        // This should be implemented with actual database check
        return ruleBuilder
            .NotEmpty()
            .Must(value => true) // Replace with actual uniqueness check
            .WithMessage("Value must be unique");
    }

    public static IRuleBuilderOptions<T, ProductImage?> MustBeValidImage<T>(
        this IRuleBuilder<T, ProductImage?> ruleBuilder)
    {
        return ruleBuilder
            .Must(ValidateImage)
            .WithMessage("Image URL is required and must be a valid URL");
    }

    public static IRuleBuilderOptions<T, ProductVideo?> MustBeValidVideo<T>(
        this IRuleBuilder<T, ProductVideo?> ruleBuilder)
    {
        return ruleBuilder
            .Must(ValidateVideo)
            .WithMessage("Video URL and thumbnail URL are required and must be valid URLs");
    }

    private static bool ValidateImage(ProductImage? image)
    {
        if (image == null) return true;
        if (string.IsNullOrEmpty(image.ImageUrl)) return false;
        return Uri.TryCreate(image.ImageUrl, UriKind.Absolute, out _);
    }

    private static bool ValidateVideo(ProductVideo? video)
    {
        if (video == null) return true;
        if (string.IsNullOrEmpty(video.VideoUrl) || string.IsNullOrEmpty(video.ThumbnailUrl)) return false;
        return Uri.TryCreate(video.VideoUrl, UriKind.Absolute, out _) &&
               Uri.TryCreate(video.ThumbnailUrl, UriKind.Absolute, out _);
    }
}
