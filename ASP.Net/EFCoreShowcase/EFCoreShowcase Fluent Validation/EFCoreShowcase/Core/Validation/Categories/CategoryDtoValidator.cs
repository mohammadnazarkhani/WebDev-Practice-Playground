using EFCoreShowcase.Constants;
using EFCoreShowcase.Core.Interfaces;
using EFCoreShowcase.Core.Validation.Base;
using EFCoreShowcase.DTOs;
using FluentValidation;

namespace EFCoreShowcase.Core.Validation.Categories;

public class CategoryDtoValidator : BaseValidator<CategoryDto>
{
    private readonly ICategoryValidator _categoryValidator;

    public CategoryDtoValidator(ICategoryValidator categoryValidator)
    {
        _categoryValidator = categoryValidator;

        ApplyCommonRules();

        RuleSet("Create", () =>
        {
            RuleFor(c => c.Name)
                .MustAsync(async (name, ct) => await _categoryValidator.IsNameUniqueAsync(name, ct))
                .WithMessage("Category name must be unique");
        });
    }

    private void ApplyCommonRules()
    {
        RuleFor(c => c.Name)
            .NotEmpty()
            .MaximumLength(ValidationConstants.Category.MaxNameLength)
            .WithMessage(ValidationMessages.Category.NameLength);
    }
}
