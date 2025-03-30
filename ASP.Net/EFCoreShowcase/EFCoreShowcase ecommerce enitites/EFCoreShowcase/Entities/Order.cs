public class Order : AuditableEntity
{
    public required string OrderNumber { get; set; }
    public new OrderStatus Status { get; set; }
    public decimal TotalAmount { get; set; }

    public long CustomerId { get; set; }
    public Customer Customer { get; set; } = null!;

    public long ShippingAddressId { get; set; }
    public Address ShippingAddress { get; set; } = null!;

    public long BillingAddressId { get; set; }
    public Address BillingAddress { get; set; } = null!;

    public ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();

    public DateTime OrderDate { get; set; }
    public DateTime? ShippedDate { get; set; }
}
