using FluentValidation;
using EFCoreShowcase.Common.Validation.Services;
using EFCoreShowcase.Entities;

namespace EFCoreShowcase.Common.Validation.Validators;

public class ProductVideoValidator : AbstractValidator<ProductVideo>
{
    private readonly IEntityExistsValidator _entityExistsValidator;

    public ProductVideoValidator(IEntityExistsValidator entityExistsValidator)
    {
        _entityExistsValidator = entityExistsValidator;

        RuleSet("Create", () =>
        {
            RuleFor(x => x.ProductId)
                .NotEmpty()
                .MustAsync(async (id, ct) => await _entityExistsValidator.ExistsAsync<Product>(id))
                .WithMessage("Product does not exist");
        });

        RuleFor(x => x.Title)
            .NotEmpty()
            .MaximumLength(200);

        RuleFor(x => x.Description)
            .NotEmpty()
            .MaximumLength(1000);

        RuleFor(x => x.VideoUrl)
            .NotEmpty()
            .Must(url => Uri.TryCreate(url, UriKind.Absolute, out _))
            .WithMessage("Invalid video URL format")
            .MaximumLength(2000);

        RuleFor(x => x.ThumbnailUrl)
            .NotEmpty()
            .Must(url => Uri.TryCreate(url, UriKind.Absolute, out _))
            .WithMessage("Invalid thumbnail URL format")
            .MaximumLength(2000);

        Include(new MediaEntityValidator<ProductVideo>());
    }
}
