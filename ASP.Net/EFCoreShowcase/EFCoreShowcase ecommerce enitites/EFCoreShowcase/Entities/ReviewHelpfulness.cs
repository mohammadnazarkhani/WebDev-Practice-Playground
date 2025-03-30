public class ReviewHelpfulness : AuditableEntity
{
    public bool IsHelpful { get; set; }

    public long ReviewId { get; set; }
    public required ProductReview Review { get; set; }

    public long CustomerId { get; set; }
    public required Customer Customer { get; set; }
}
