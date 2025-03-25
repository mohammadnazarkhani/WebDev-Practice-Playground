using ImageServer.Api.Models;

namespace ImageServer.Api.DTOs.Images.Responses;

/// <summary>
/// Data transfer object for image responses
/// </summary>
/// <param name="Id">Unique identifier of the image</param>
/// <param name="Name">Display name of the image</param>
/// <param name="ContentType">MIME type of the image</param>
/// <param name="FileSize">Size of the image file in bytes</param>
/// <param name="CreatedAt">UTC timestamp when the image was created</param>
/// <param name="UpdatedAt">UTC timestamp when the image was last updated</param>
/// <param name="ImageUrl">URL to access the full-size image</param>
/// <param name="ThumbnailUrl">URL to access the thumbnail version of the image</param>
public record ImageResponseDto(
    Guid Id,
    string Name,
    string ContentType,
    long FileSize,
    DateTime CreatedAt,
    DateTime? UpdatedAt,
    string ImageUrl,
    string ThumbnailUrl);
