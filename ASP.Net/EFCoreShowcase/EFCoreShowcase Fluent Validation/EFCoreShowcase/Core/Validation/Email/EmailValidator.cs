using EFCoreShowcase.Core.Validation.Base;
using EFCoreShowcase.Models;
using FluentValidation;

namespace EFCoreShowcase.Core.Validation.Email;

public class EmailValidator : BaseValidator<Models.Email>
{
    public EmailValidator()
    {
        RuleFor(x => x.Address)
            .NotEmpty()
            .WithMessage("Email address is required")
            .EmailAddress()
            .WithMessage("Invalid email address format")
            .Matches(@"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$")
            .WithMessage("Email must be in a valid format");

        RuleFor(x => x.Subject)
            .NotEmpty()
            .WithMessage("Subject is required")
            .MaximumLength(100)
            .WithMessage("Subject cannot exceed 100 characters");

        RuleFor(x => x.Message)
            .NotEmpty()
            .WithMessage("Message is required")
            .MaximumLength(1000)
            .WithMessage("Message cannot exceed 1000 characters");
    }
}
