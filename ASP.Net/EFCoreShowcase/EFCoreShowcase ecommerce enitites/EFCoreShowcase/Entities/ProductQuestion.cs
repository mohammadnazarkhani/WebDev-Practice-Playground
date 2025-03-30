public class ProductQuestion : AuditableEntity
{
    public required string QuestionText { get; set; }

    public long CustomerId { get; set; }
    public required Customer Customer { get; set; }

    public long ProductId { get; set; }
    public required Product Product { get; set; }

    public ICollection<ProductAnswer> Answers { get; set; } = new List<ProductAnswer>();
}
