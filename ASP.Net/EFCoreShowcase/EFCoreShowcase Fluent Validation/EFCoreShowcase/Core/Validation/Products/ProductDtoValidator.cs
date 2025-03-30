using EFCoreShowcase.Constants;
using EFCoreShowcase.Core.Interfaces;
using EFCoreShowcase.Core.Validation.Base;
using EFCoreShowcase.DTOs;
using FluentValidation;

namespace EFCoreShowcase.Core.Validation.Products;

public class ProductDtoValidator : BaseValidator<ProductDto>
{
    private readonly ICategoryValidator _categoryValidator;

    public ProductDtoValidator(ICategoryValidator categoryValidator)
    {
        _categoryValidator = categoryValidator;

        ApplyRuleSet("Create", () =>
        {
            ApplyCommonRules();
            ValidateCategoryExists();
        });

        ApplyRuleSet("Update", () =>
        {
            ApplyCommonRules();
            ValidateCategoryExists();
        });
    }

    private void ApplyCommonRules()
    {
        RuleFor(p => p.Name)
            .NotEmpty()
            .MaximumLength(ValidationConstants.Product.MaxNameLength)
            .WithMessage(ValidationMessages.Product.NameLength);

        RuleFor(p => p.Price)
            .GreaterThan(ValidationConstants.Product.MinPrice)
            .PrecisionScale(18, 2, false)
            .WithMessage(ValidationMessages.Product.PricePositive);

        RuleFor(p => p.Description)
            .MaximumLength(ValidationConstants.Product.MaxDescriptionLength)
            .When(x => !string.IsNullOrEmpty(x.Description))
            .WithMessage(ValidationMessages.Product.DescriptionLength);
    }

    private void ValidateCategoryExists()
    {
        RuleFor(p => p.CategoryId)
            .NotEmpty()
            .MustAsync(async (id, ct) => await _categoryValidator.CategoryExistsAsync(id, ct))
            .WithMessage(ValidationMessages.Product.CategoryRequired);
    }
}
