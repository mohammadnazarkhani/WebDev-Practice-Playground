using FluentValidation;
using EFCoreShowcase.Common.Extensions;
using EFCoreShowcase.Common.Validation.Services;
using EFCoreShowcase.Entities;

namespace EFCoreShowcase.Common.Validation.Validators;

public class CustomerValidator : AbstractValidator<Customer>
{
    private readonly IUniquenessValidator _uniquenessValidator;

    public CustomerValidator(IUniquenessValidator uniquenessValidator)
    {
        _uniquenessValidator = uniquenessValidator;

        RuleSet("Create", () =>
        {
            RuleFor(x => x.Email)
                .MustAsync(async (email, ct) => await _uniquenessValidator.IsUniqueAsync<Customer>("Email", email))
                .WithMessage("Email address is already in use");
        });

        RuleFor(x => x.FirstName)
            .NotEmpty()
            .MaximumLength(50);

        RuleFor(x => x.LastName)
            .NotEmpty()
            .MaximumLength(50);

        RuleFor(x => x.Email)
            .NotEmpty()
            .MustBeValidEmail();

        RuleFor(x => x.PhoneNumber)
            .NotEmpty()
            .MustBeValidPhone();
    }
}
