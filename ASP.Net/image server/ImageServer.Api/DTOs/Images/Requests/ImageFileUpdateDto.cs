using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace ImageServer.Api.DTOs.Images.Requests;

/// <summary>
/// Data transfer object for updating an existing image
/// </summary>
public record ImageFileUpdateDto
{
    /// <summary>
    /// New display name for the image (optional)
    /// </summary>
    [FromForm(Name = "name")]
    public string? Name { get; init; }

    /// <summary>
    /// New image file to replace the existing one (optional)
    /// </summary>
    [FromForm(Name = "file")]
    public IFormFile? File { get; init; }
}
