using FluentValidation;
using EFCoreShowcase.Entities;
using EFCoreShowcase.Common.Validation.Services;

namespace EFCoreShowcase.Common.Validation.Validators;

public class AddressValidator : AbstractValidator<Address>
{
    private readonly IEntityExistsValidator _entityExistsValidator;

    public AddressValidator(IEntityExistsValidator entityExistsValidator)
    {
        _entityExistsValidator = entityExistsValidator;

        RuleSet("Create", () =>
        {
            RuleFor(x => x.CustomerId)
                .NotEmpty()
                .MustAsync(async (id, ct) => await _entityExistsValidator.ExistsAsync<Customer>(id))
                .WithMessage("Customer does not exist");
        });

        RuleFor(x => x.Street)
            .NotEmpty()
            .MinimumLength(5).WithMessage("Street must be at least 5 characters")
            .MaximumLength(200);

        RuleFor(x => x.City)
            .NotEmpty()
            .MinimumLength(2).WithMessage("City must be at least 2 characters")
            .MaximumLength(100)
            .Must(city => !city.Any(char.IsDigit)).WithMessage("City name cannot contain numbers");

        RuleFor(x => x.State)
            .NotEmpty()
            .MinimumLength(2).WithMessage("State must be at least 2 characters")
            .MaximumLength(100)
            .Must(state => !state.Any(char.IsDigit)).WithMessage("State name cannot contain numbers");

        RuleFor(x => x.Country)
            .NotEmpty()
            .MinimumLength(2).WithMessage("Country must be at least 2 characters")
            .MaximumLength(100)
            .Must(country => !country.Any(char.IsDigit)).WithMessage("Country name cannot contain numbers");

        RuleFor(x => x.PostalCode)
            .NotEmpty()
            .MaximumLength(20)
            .Matches(@"^[0-9a-zA-Z-\s]*$").WithMessage("Postal code can only contain letters, numbers, spaces and hyphens");
    }
}
