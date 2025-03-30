using FluentValidation;
using EFCoreShowcase.Entities;
using EFCoreShowcase.Common.Validation.Services;

namespace EFCoreShowcase.Common.Validation.Validators;

public class ProductAnswerValidator : AbstractValidator<ProductAnswer>
{
    private readonly IEntityExistsValidator _entityExistsValidator;

    public ProductAnswerValidator(IEntityExistsValidator entityExistsValidator)
    {
        _entityExistsValidator = entityExistsValidator;

        RuleSet("Create", () =>
        {
            RuleFor(x => x.CustomerId)
                .NotEmpty()
                .MustAsync(async (id, ct) => await _entityExistsValidator.ExistsAsync<Customer>(id))
                .WithMessage("Customer does not exist");

            RuleFor(x => x.QuestionId)
                .NotEmpty()
                .MustAsync(async (id, ct) => await _entityExistsValidator.ExistsAsync<ProductQuestion>(id))
                .WithMessage("Question does not exist");
        });

        RuleFor(x => x.AnswerText)
            .NotEmpty().WithMessage("Answer text is required")
            .MinimumLength(10).WithMessage("Answer must be at least 10 characters")
            .MaximumLength(1000).WithMessage("Answer cannot exceed 1000 characters")
            .Must(text => !text.Contains("  ")).WithMessage("Answer cannot contain consecutive spaces");
    }
}
