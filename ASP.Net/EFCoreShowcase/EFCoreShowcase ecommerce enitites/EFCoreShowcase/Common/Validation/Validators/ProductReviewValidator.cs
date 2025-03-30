using FluentValidation;
using EFCoreShowcase.Common.Validation.Services;
using EFCoreShowcase.Entities;

namespace EFCoreShowcase.Common.Validation.Validators;

public class ProductReviewValidator : AbstractValidator<ProductReview>
{
    private readonly IEntityExistsValidator _entityExistsValidator;

    public ProductReviewValidator(IEntityExistsValidator entityExistsValidator)
    {
        _entityExistsValidator = entityExistsValidator;

        RuleSet("Create", () =>
        {
            RuleFor(x => x.CustomerId)
                .NotEmpty()
                .MustAsync(async (id, ct) => await _entityExistsValidator.ExistsAsync<Customer>(id))
                .WithMessage("Customer does not exist");

            RuleFor(x => x.ProductId)
                .NotEmpty()
                .MustAsync(async (id, ct) => await _entityExistsValidator.ExistsAsync<Product>(id))
                .WithMessage("Product does not exist");
        });

        RuleFor(x => x.Title)
            .NotEmpty()
            .MaximumLength(200)
            .Must(title => !title.Contains("  "))
            .WithMessage("Title cannot contain consecutive spaces");

        RuleFor(x => x.Content)
            .NotEmpty()
            .MinimumLength(10)
            .MaximumLength(2000)
            .Must(content => !content.Contains("  "))
            .WithMessage("Content cannot contain consecutive spaces");

        RuleFor(x => x.Rating)
            .InclusiveBetween(1, 5)
            .WithMessage("Rating must be between 1 and 5");

        RuleForEach(x => x.Images)
            .SetValidator(new ReviewImageValidator(entityExistsValidator));
    }
}
