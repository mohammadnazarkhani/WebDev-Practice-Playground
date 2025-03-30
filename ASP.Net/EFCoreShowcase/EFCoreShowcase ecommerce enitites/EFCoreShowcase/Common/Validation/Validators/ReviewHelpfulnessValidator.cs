using FluentValidation;
using EFCoreShowcase.Common.Validation.Services;
using EFCoreShowcase.Entities;

public class ReviewHelpfulnessValidator : AbstractValidator<ReviewHelpfulness>
{
    private readonly IEntityExistsValidator _entityExistsValidator;

    public ReviewHelpfulnessValidator(IEntityExistsValidator entityExistsValidator)
    {
        _entityExistsValidator = entityExistsValidator;

        RuleSet("Create", () =>
        {
            RuleFor(x => x.ReviewId)
                .NotEmpty()
                .MustAsync(async (id, ct) => await _entityExistsValidator.ExistsAsync<ProductReview>(id))
                .WithMessage("Review does not exist");

            RuleFor(x => x.CustomerId)
                .NotEmpty()
                .MustAsync(async (id, ct) => await _entityExistsValidator.ExistsAsync<Customer>(id))
                .WithMessage("Customer does not exist");

            RuleFor(x => x)
                .MustAsync(async (rh, ct) => !await _entityExistsValidator.ExistsAsync<ReviewHelpfulness>(rh.Id))
                .WithMessage("Customer has already voted for this review");
        });
    }
}
