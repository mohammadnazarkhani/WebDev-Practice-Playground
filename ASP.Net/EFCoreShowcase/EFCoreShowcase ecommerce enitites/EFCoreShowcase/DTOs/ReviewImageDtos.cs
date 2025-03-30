using System.ComponentModel.DataAnnotations;

namespace EFCoreShowcase.DTOs;

public record ReviewImageDto
{
    public Guid Id { get; init; }  // Change from long to Guid
    public string ImageUrl { get; init; } = null!;
    public DateTime CreatedAt { get; init; }
}

public record CreateReviewImageDto
{
    [Required, Url]
    public string ImageUrl { get; init; } = null!;

    [Required]
    public long ReviewId { get; init; }
}
