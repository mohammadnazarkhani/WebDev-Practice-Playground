using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace ImageServer.Api.DTOs.Images.Requests;

/// <summary>
/// Data transfer object for image upload requests
/// </summary>
public record ImageUploadDto
{
    /// <summary>
    /// Display name for the image
    /// </summary>
    [FromForm(Name = "name")]
    [Required(ErrorMessage = "Name is required")]
    public string Name { get; init; } = string.Empty;

    /// <summary>
    /// The image file to be uploaded
    /// </summary>
    [FromForm(Name = "file")]
    [Required(ErrorMessage = "File is required")]
    public IFormFile File { get; init; } = default!;
}
