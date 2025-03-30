public class ProductReview : AuditableEntity
{
    public required string Title { get; set; }
    public required string Content { get; set; }
    public int Rating { get; set; }
    public bool IsVerifiedPurchase { get; set; }

    public long CustomerId { get; set; }
    public required Customer Customer { get; set; }

    public long ProductId { get; set; }
    public required Product Product { get; set; }

    public ICollection<ReviewImage> Images { get; set; } = new List<ReviewImage>();
    public ICollection<ReviewHelpfulness> Helpfulness { get; set; } = new List<ReviewHelpfulness>();
}
