public class ProductImage : MediaEntityBase
{
    public required string ImageUrl { get; set; }
    public long ProductId { get; set; }
    public Product Product { get; set; } = null!;
}
