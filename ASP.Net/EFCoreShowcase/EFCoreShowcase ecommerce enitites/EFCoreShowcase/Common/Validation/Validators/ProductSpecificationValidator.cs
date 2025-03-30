using FluentValidation;
using EFCoreShowcase.Common.Validation.Services;
using EFCoreShowcase.Entities;

public class ProductSpecificationValidator : AbstractValidator<ProductSpecification>
{
    private readonly IEntityExistsValidator _entityExistsValidator;

    public ProductSpecificationValidator(IEntityExistsValidator entityExistsValidator)
    {
        _entityExistsValidator = entityExistsValidator;

        RuleSet("Create", () =>
        {
            RuleFor(x => x.ProductId)
                .NotEmpty()
                .MustAsync(async (id, ct) => await _entityExistsValidator.ExistsAsync<Product>(id))
                .WithMessage("Product does not exist");
        });

        RuleFor(x => x.SpecificationKey)
            .NotEmpty()
            .MaximumLength(100)
            .Matches(@"^[a-zA-Z0-9\s-_]+$")
            .WithMessage("Specification key can only contain letters, numbers, spaces, hyphens and underscores");

        RuleFor(x => x.SpecificationValue)
            .NotEmpty()
            .MaximumLength(500);

        RuleFor(x => x.Group)
            .NotEmpty()
            .MaximumLength(100)
            .Matches(@"^[a-zA-Z0-9\s-_]+$")
            .WithMessage("Group name can only contain letters, numbers, spaces, hyphens and underscores");

        RuleFor(x => x.DisplayOrder)
            .GreaterThanOrEqualTo(0);
    }
}
