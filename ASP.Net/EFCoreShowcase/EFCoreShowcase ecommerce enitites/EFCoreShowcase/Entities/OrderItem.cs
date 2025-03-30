public class OrderItem : AuditableEntity
{
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }

    public long OrderId { get; set; }
    public required Order Order { get; set; }

    public long ProductId { get; set; }
    public required Product Product { get; set; }
}
