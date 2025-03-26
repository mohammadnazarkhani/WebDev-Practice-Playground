namespace ImageServer.Api.DTOs.Images.Responses;

/// <summary>
/// Detailed response DTO for image information including file details
/// </summary>
public record ImageDetailsResponseDto(
    Guid Id,
    string Name,
    string ContentType,
    long FileSize,
    DateTime CreatedAt,
    DateTime? UpdatedAt,
    string ImageUrl,
    string ThumbnailUrl,
    string FilePath,
    string? ThumbnailPath,
    IDictionary<string, object> Metadata
);
