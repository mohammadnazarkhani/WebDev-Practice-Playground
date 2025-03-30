using System.ComponentModel.DataAnnotations;

namespace EFCoreShowcase.DTOs;

public record ProductImageDto
{
    public Guid Id { get; init; }
    public string ImageUrl { get; init; } = null!;
    public DateTime CreatedAt { get; init; }
}

public record ProductVideoDto
{
    public Guid Id { get; init; }
    public string Title { get; init; } = null!;
    public string VideoUrl { get; init; } = null!;
    public string ThumbnailUrl { get; init; } = null!;
    public string Description { get; init; } = null!;
    public DateTime CreatedAt { get; init; }
}

public record CreateProductMediaDto
{
    [Required, Url]
    public string Url { get; init; } = null!;

    [StringLength(200)]
    public string? Title { get; init; }

    [StringLength(500)]
    public string? Description { get; init; }
}
