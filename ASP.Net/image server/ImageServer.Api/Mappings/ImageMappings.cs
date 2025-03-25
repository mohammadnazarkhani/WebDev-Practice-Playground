using ImageServer.Api.Models;
using ImageServer.Api.DTOs.Images.Responses;

namespace ImageServer.Api.Mappings;

/// <summary>
/// Extension methods for mapping Image entities to DTOs
/// </summary>
public static class ImageMappings
{
    /// <summary>
    /// Converts an Image entity to an ImageResponseDto
    /// </summary>
    /// <param name="image">The image entity to convert</param>
    /// <param name="httpContextAccessor">HTTP context accessor for building URLs</param>
    /// <returns>An ImageResponseDto containing the image data and URLs</returns>
    public static ImageResponseDto ToDto(this Image image, IHttpContextAccessor httpContextAccessor)
    {
        var request = httpContextAccessor.HttpContext?.Request;
        var baseUrl = $"{request?.Scheme}://{request?.Host}";

        return new ImageResponseDto(
            image.Id,
            image.Name,
            image.ContentType,
            image.FileSize,
            image.CreatedAt,
            image.UpdatedAt,
            $"{baseUrl}/images/{image.Id}",
            $"{baseUrl}/images/{image.Id}/thumbnail"
        );
    }
}
