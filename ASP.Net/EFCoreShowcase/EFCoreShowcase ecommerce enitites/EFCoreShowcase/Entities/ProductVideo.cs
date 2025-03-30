public class ProductVideo : MediaEntityBase
{
    public required string Title { get; set; }
    public required string VideoUrl { get; set; }
    public required string ThumbnailUrl { get; set; }
    public required string Description { get; set; }

    public long ProductId { get; set; }
    public Product Product { get; set; } = null!;
}
