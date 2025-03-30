using FluentValidation;
using EFCoreShowcase.Entities;
using EFCoreShowcase.Common.Validation.Services;

namespace EFCoreShowcase.Common.Validation.Validators;

public class CategoryValidator : AbstractValidator<Category>
{
    private readonly IEntityExistsValidator _entityExistsValidator;
    private readonly IUniquenessValidator _uniquenessValidator;

    public CategoryValidator(
        IEntityExistsValidator entityExistsValidator,
        IUniquenessValidator uniquenessValidator)
    {
        _entityExistsValidator = entityExistsValidator;
        _uniquenessValidator = uniquenessValidator;

        RuleSet("Create", () =>
        {
            RuleFor(x => x.Name)
                .NotEmpty()
                .MinimumLength(2).WithMessage("Category name must be at least 2 characters")
                .MaximumLength(100)
                .MustAsync(async (name, ct) => await _uniquenessValidator.IsUniqueAsync<Category>("Name", name))
                .WithMessage("Category name must be unique");

            RuleFor(x => x.ParentCategoryId)
                .MustAsync(async (id, ct) => !id.HasValue || await _entityExistsValidator.ExistsAsync<Category>(id.Value))
                .WithMessage("Parent category does not exist")
                .When(x => x.ParentCategoryId.HasValue);
        });

        RuleFor(x => x.Description)
            .NotEmpty()
            .MaximumLength(1000);
    }
}
