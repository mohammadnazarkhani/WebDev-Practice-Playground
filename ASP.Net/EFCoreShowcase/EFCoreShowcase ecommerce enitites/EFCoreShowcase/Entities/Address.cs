public class Address : AuditableEntity
{
    public required string Street { get; set; }
    public required string City { get; set; }
    public required string State { get; set; }
    public required string Country { get; set; }
    public required string PostalCode { get; set; }

    public long CustomerId { get; set; }
    public required Customer Customer { get; set; }

    public ICollection<Order> ShippingOrders { get; set; } = new List<Order>();
    public ICollection<Order> BillingOrders { get; set; } = new List<Order>();
}
