using FluentValidation;
using EFCoreShowcase.Common.Extensions;
using EFCoreShowcase.Common.Validation.Services;
using EFCoreShowcase.Entities;

namespace EFCoreShowcase.Common.Validation.Validators;

public class OrderItemValidator : AbstractValidator<OrderItem>
{
    private readonly IEntityExistsValidator _entityExistsValidator;

    public OrderItemValidator(IEntityExistsValidator entityExistsValidator)
    {
        _entityExistsValidator = entityExistsValidator;

        RuleSet("Create", () =>
        {
            RuleFor(x => x.ProductId)
                .NotEmpty()
                .MustAsync(async (id, ct) => await _entityExistsValidator.ExistsAsync<Product>(id))
                .WithMessage("Product does not exist");
        });

        RuleFor(x => x.Quantity)
            .GreaterThan(0).WithMessage("Quantity must be greater than 0")
            .LessThanOrEqualTo(1000).WithMessage("Quantity cannot exceed 1000 items");

        RuleFor(x => x.UnitPrice)
            .MustBePositiveMoney()
            .PrecisionScale(18, 2, false)
            .LessThanOrEqualTo(1000000).WithMessage("Unit price cannot exceed 1,000,000");
    }
}
