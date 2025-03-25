namespace ImageServer.Api.Models;

/// <summary>
/// Represents an image entity in the system, extending the base file entity with image-specific properties
/// </summary>
public class Image : BaseFileEntity
{
    /// <summary>
    /// Physical path to the thumbnail version of the image
    /// </summary>
    public string? ThumbnailPath { get; set; }
}
