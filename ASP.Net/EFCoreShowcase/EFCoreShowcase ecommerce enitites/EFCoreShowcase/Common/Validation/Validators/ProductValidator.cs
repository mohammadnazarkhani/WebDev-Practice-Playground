using FluentValidation;
using EFCoreShowcase.Common.Extensions;
using EFCoreShowcase.Common.Validation.Services;
using EFCoreShowcase.Entities;

namespace EFCoreShowcase.Common.Validation.Validators;

public class ProductValidator : AbstractValidator<Product>
{
    private readonly IUniquenessValidator _uniquenessValidator;
    private readonly IEntityExistsValidator _entityExistsValidator;

    public ProductValidator(
        IUniquenessValidator uniquenessValidator,
        IEntityExistsValidator entityExistsValidator)
    {
        _uniquenessValidator = uniquenessValidator;
        _entityExistsValidator = entityExistsValidator;

        RuleSet("Create", () =>
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Product name is required")
                .Length(3, 200).WithMessage("Product name must be between {MinLength} and {MaxLength} characters")
                .MustBeValidProductName();

            RuleFor(x => x.Description)
                .NotEmpty().WithMessage("Product description is required")
                .Length(10, 2000).WithMessage("Product description must be between {MinLength} and {MaxLength} characters");

            RuleFor(x => x.SKU)
                .NotEmpty().WithMessage("SKU is required")
                .Matches(@"^[A-Z]{2}-\d{6}$").WithMessage("SKU must be in format: XX-000000")
                .MustAsync(async (sku, ct) => await _uniquenessValidator.IsUniqueAsync<Product>("SKU", sku))
                .WithMessage("SKU must be unique");

            RuleFor(x => x.CategoryId)
                .NotEmpty().WithMessage("Category is required")
                .MustAsync(async (id, ct) => await _entityExistsValidator.ExistsAsync<Category>(id))
                .WithMessage("Category does not exist");
        });

        RuleSet("Update", () =>
        {
            RuleFor(x => x.Name)
                .Length(3, 200).WithMessage("Product name must be between {MinLength} and {MaxLength} characters")
                .MustBeValidProductName()
                .When(x => !string.IsNullOrEmpty(x.Name));

            RuleFor(x => x.Description)
                .Length(10, 2000).WithMessage("Product description must be between {MinLength} and {MaxLength} characters")
                .When(x => !string.IsNullOrEmpty(x.Description));
        });

        // Common rules for both Create and Update
        RuleFor(x => x.Price.Amount)
            .GreaterThan(0).WithMessage("Price must be greater than 0")
            .PrecisionScale(18, 2, false).WithMessage("Price cannot have more than 2 decimal places")
            .LessThanOrEqualTo(1000000).WithMessage("Price cannot exceed 1,000,000");

        RuleFor(x => x.Price.Currency)
            .NotEmpty().WithMessage("Currency is required")
            .Length(3).WithMessage("Currency code must be 3 characters")
            .Matches("^[A-Z]{3}$").WithMessage("Currency must be in ISO 4217 format (e.g., USD, EUR)");

        RuleFor(x => x.StockQuantity)
            .GreaterThanOrEqualTo(0).WithMessage("Stock quantity cannot be negative")
            .LessThanOrEqualTo(1000000).WithMessage("Stock quantity cannot exceed 1,000,000");

        RuleFor(x => x.AverageRating)
            .InclusiveBetween(0, 5).WithMessage("Average rating must be between 0 and 5");

        RuleFor(x => x.MainImage)
            .MustBeValidImage()
            .When(x => x.MainImage != null);

        RuleFor(x => x.MainVideo)
            .MustBeValidVideo()
            .When(x => x.MainVideo != null);

        RuleFor(x => x)
            .Must(x => !x.IsLowStock || x.StockQuantity <= x.MinStockThreshold)
            .WithMessage("Low stock flag is inconsistent with stock quantity");
    }
}

public static class ProductValidatorExtensions
{
    public static IRuleBuilderOptions<T, string> MustBeValidProductName<T>(this IRuleBuilder<T, string> ruleBuilder)
    {
        return ruleBuilder.Must(name => !name.Contains("  "))
            .WithMessage("Product name cannot contain consecutive spaces")
            .Must(name => !ContainsSpecialCharacters(name))
            .WithMessage("Product name cannot contain special characters");
    }

    private static bool ContainsSpecialCharacters(string value)
    {
        return value.Any(ch => !char.IsLetterOrDigit(ch) && !char.IsWhiteSpace(ch) && ch != '-' && ch != '_');
    }
}
