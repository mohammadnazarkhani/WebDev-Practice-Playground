public class ReviewImage : MediaEntityBase
{
    public required string ImageUrl { get; set; }
    public long ReviewId { get; set; }
    public ProductReview Review { get; set; } = null!;
}
