using FluentValidation;
using EFCoreShowcase.Entities;
using EFCoreShowcase.Common.Validation.Services;

namespace EFCoreShowcase.Common.Validation.Validators;

public class ProductQuestionValidator : AbstractValidator<ProductQuestion>
{
    private readonly IEntityExistsValidator _entityExistsValidator;

    public ProductQuestionValidator(IEntityExistsValidator entityExistsValidator)
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

        RuleFor(x => x.QuestionText)
            .NotEmpty().WithMessage("Question text is required")
            .MinimumLength(10).WithMessage("Question must be at least 10 characters")
            .MaximumLength(1000).WithMessage("Question cannot exceed 1000 characters")
            .Must(text => !text.Contains("  ")).WithMessage("Question cannot contain consecutive spaces");
    }
}
