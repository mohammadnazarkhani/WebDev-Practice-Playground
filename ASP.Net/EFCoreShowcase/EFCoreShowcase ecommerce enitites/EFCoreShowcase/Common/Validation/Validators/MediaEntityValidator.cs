using FluentValidation;
using EFCoreShowcase.Entities;

namespace EFCoreShowcase.Common.Validation.Validators;

public class MediaEntityValidator<T> : AbstractValidator<T> where T : MediaEntityBase
{
    public MediaEntityValidator()
    {
        RuleFor(x => x.CreatedAt)
            .NotEmpty()
            .LessThanOrEqualTo(DateTime.UtcNow)
            .WithMessage("Created date cannot be in the future");

        RuleFor(x => x.UpdatedAt)
            .Must((entity, updatedAt) => !updatedAt.HasValue || updatedAt.Value > entity.CreatedAt)
            .WithMessage("Updated date must be later than created date")
            .When(x => x.UpdatedAt.HasValue);

        RuleFor(x => x.DeletedAt)
            .Must((entity, deletedAt) => !deletedAt.HasValue || deletedAt.Value > entity.CreatedAt)
            .WithMessage("Deleted date must be later than created date")
            .Must((entity, deletedAt) => !deletedAt.HasValue || !entity.UpdatedAt.HasValue || deletedAt.Value > entity.UpdatedAt.Value)
            .WithMessage("Deleted date must be later than updated date")
            .When(x => x.DeletedAt.HasValue);
    }
}
