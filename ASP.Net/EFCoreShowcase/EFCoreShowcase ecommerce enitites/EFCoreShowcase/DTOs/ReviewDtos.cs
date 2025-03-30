using System.ComponentModel.DataAnnotations;

namespace EFCoreShowcase.DTOs;

public record ProductReviewDto
{
    public long Id { get; init; }
    public string Title { get; init; } = null!;
    public string Content { get; init; } = null!;
    public int Rating { get; init; }
    public string CustomerName { get; init; } = null!;
    public bool IsVerifiedPurchase { get; init; }
    public DateTime CreatedAt { get; init; }
    public ICollection<string> ImageUrls { get; init; } = new List<string>();
}

public record CreateReviewDto
{
    [Required, StringLength(200)]
    public string Title { get; init; } = null!;

    [Required, StringLength(2000)]
    public string Content { get; init; } = null!;

    [Range(1, 5)]
    public int Rating { get; init; }

    [Required]
    public long ProductId { get; init; }

    public List<string> ImageUrls { get; init; } = new();
}
