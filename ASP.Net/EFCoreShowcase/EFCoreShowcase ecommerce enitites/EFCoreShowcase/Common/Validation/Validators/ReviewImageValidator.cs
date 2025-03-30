using FluentValidation;
using EFCoreShowcase.Common.Extensions;
using EFCoreShowcase.Common.Validation.Services;
using EFCoreShowcase.Entities;

public class ReviewImageValidator : AbstractValidator<ReviewImage>
{
    private readonly IEntityExistsValidator _entityExistsValidator;

    public ReviewImageValidator(IEntityExistsValidator entityExistsValidator)
    {
        _entityExistsValidator = entityExistsValidator;

        RuleSet("Create", () =>
        {
            RuleFor(x => x.ReviewId)
                .NotEmpty()
                .MustAsync(async (id, ct) => await _entityExistsValidator.ExistsAsync<ProductReview>(id))
                .WithMessage("Review does not exist");
        });

        RuleFor(x => x.ImageUrl)
            .NotEmpty()
            .Must(url => Uri.TryCreate(url, UriKind.Absolute, out _))
            .WithMessage("Invalid image URL format")
            .MaximumLength(2000);
    }
}
