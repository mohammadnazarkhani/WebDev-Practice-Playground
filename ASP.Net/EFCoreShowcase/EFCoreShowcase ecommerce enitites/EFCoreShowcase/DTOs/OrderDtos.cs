using System.ComponentModel.DataAnnotations;
using EFCoreShowcase.Entities;

namespace EFCoreShowcase.DTOs;

public record OrderDto
{
    public long Id { get; init; }
    public string OrderNumber { get; init; } = null!;
    public OrderStatus Status { get; init; }
    public decimal TotalAmount { get; init; }
    public string CustomerName { get; init; } = null!;
    public DateTime OrderDate { get; init; }
    public DateTime? ShippedDate { get; init; }
    public ICollection<OrderItemDto> Items { get; init; } = new List<OrderItemDto>();
}

public record OrderItemDto
{
    public long ProductId { get; init; }
    public string ProductName { get; init; } = null!;
    public int Quantity { get; init; }
    public decimal UnitPrice { get; init; }
}

public record CreateOrderDto
{
    [Required]
    public long CustomerId { get; init; }

    [Required]
    public long ShippingAddressId { get; init; }

    [Required]
    public long BillingAddressId { get; init; }

    [Required]
    public List<CreateOrderItemDto> Items { get; init; } = new();
}

public record CreateOrderItemDto
{
    [Required]
    public long ProductId { get; init; }

    [Range(1, int.MaxValue)]
    public int Quantity { get; init; }
}

// Add order status update DTO
public record UpdateOrderStatusDto
{
    [Required]
    public OrderStatus Status { get; init; }

    [StringLength(500)]
    public string? StatusNote { get; init; }
}

// Add order summary DTO for list views
public record OrderSummaryDto
{
    public long Id { get; init; }
    public string OrderNumber { get; init; } = null!;
    public OrderStatus Status { get; init; }
    public decimal TotalAmount { get; init; }
    public DateTime OrderDate { get; init; }
}
