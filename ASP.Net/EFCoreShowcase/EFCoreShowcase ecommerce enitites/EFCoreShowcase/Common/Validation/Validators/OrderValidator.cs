using FluentValidation;
using EFCoreShowcase.Common.Extensions;
using EFCoreShowcase.Common.Validation.Services;
using EFCoreShowcase.Entities;

namespace EFCoreShowcase.Common.Validation.Validators;

public class OrderValidator : AbstractValidator<Order>
{
    private readonly IEntityExistsValidator _entityExistsValidator;
    private readonly IDictionary<OrderStatus, OrderStatus[]> _validTransitions;

    public OrderValidator(IEntityExistsValidator entityExistsValidator)
    {
        _entityExistsValidator = entityExistsValidator;
        _validTransitions = InitializeValidTransitions();

        RuleSet("Create", () =>
        {
            RuleFor(x => x.CustomerId)
                .NotEmpty()
                .MustAsync(async (id, ct) => await _entityExistsValidator.ExistsAsync<Customer>(id))
                .WithMessage("Customer does not exist");

            RuleFor(x => x.ShippingAddressId)
                .NotEmpty().WithMessage("Shipping address is required")
                .MustAsync(async (id, ct) => await _entityExistsValidator.ExistsAsync<Address>(id))
                .WithMessage("Shipping address does not exist");

            RuleFor(x => x.BillingAddressId)
                .NotEmpty().WithMessage("Billing address is required")
                .MustAsync(async (id, ct) => await _entityExistsValidator.ExistsAsync<Address>(id))
                .WithMessage("Billing address does not exist");
        });

        RuleFor(x => x.OrderNumber)
            .NotEmpty().WithMessage("Order number is required")
            .MaximumLength(50).WithMessage("Order number cannot exceed {MaxLength} characters")
            .Matches(@"^ORD-\d{8}-[a-zA-Z0-9]{8}$").WithMessage("Invalid order number format");

        RuleFor(x => x.TotalAmount)
            .GreaterThan(0).WithMessage("Total amount must be greater than 0")
            .PrecisionScale(18, 2, false).WithMessage("Total amount cannot have more than 2 decimal places");

        RuleFor(x => x.OrderItems)
            .NotEmpty().WithMessage("Order must have at least one item")
            .Must(items => items.Count <= 100).WithMessage("Order cannot have more than 100 items");

        RuleFor(x => x.OrderDate)
            .NotEmpty().WithMessage("Order date is required")
            .LessThanOrEqualTo(DateTime.UtcNow).WithMessage("Order date cannot be in the future");

        RuleFor(x => x.ShippedDate)
            .Must((order, shippedDate) => !shippedDate.HasValue || shippedDate.Value > order.OrderDate)
            .WithMessage("Shipped date must be after order date")
            .When(x => x.ShippedDate.HasValue);

        RuleFor(x => x)
            .Must(ValidateShippingStatus)
            .WithMessage("Shipped date is required when order status is Shipped")
            .Must(ValidateStatusTransition)
            .WithMessage("Invalid order status transition");
    }

    private bool ValidateShippingStatus(Order order)
        => order.Status != OrderStatus.Shipped || order.ShippedDate.HasValue;

    private bool ValidateStatusTransition(Order order)
    {
        if (order.Status == OrderStatus.Pending) return true;

        var previousStatus = GetPreviousStatus(order);
        return _validTransitions.TryGetValue(previousStatus, out var validNextStates) &&
               validNextStates.Contains(order.Status);
    }

    private static IDictionary<OrderStatus, OrderStatus[]> InitializeValidTransitions()
    {
        return new Dictionary<OrderStatus, OrderStatus[]>
        {
            [OrderStatus.Pending] = new[] { OrderStatus.Processing, OrderStatus.Cancelled },
            [OrderStatus.Processing] = new[] { OrderStatus.Shipped, OrderStatus.Cancelled },
            [OrderStatus.Shipped] = new[] { OrderStatus.Delivered },
            [OrderStatus.Delivered] = Array.Empty<OrderStatus>(),
            [OrderStatus.Cancelled] = Array.Empty<OrderStatus>()
        };
    }

    private OrderStatus GetPreviousStatus(Order order)
    {
        // Implementation to get previous status from audit trail or domain events
        return OrderStatus.Pending; // Default implementation
    }
}
