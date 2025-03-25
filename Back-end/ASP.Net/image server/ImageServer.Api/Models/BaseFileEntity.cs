namespace ImageServer.Api.Models;

/// <summary>
/// Base class for file-based entities providing common properties for file management
/// </summary>
public abstract class BaseFileEntity
{
    /// <summary>
    /// Unique identifier for the entity
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Display name of the file
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Physical path where the file is stored on disk
    /// </summary>
    public string FilePath { get; set; } = string.Empty;

    /// <summary>
    /// MIME type of the file
    /// </summary>
    public string ContentType { get; set; } = string.Empty;

    /// <summary>
    /// Size of the file in bytes
    /// </summary>
    public long FileSize { get; set; }

    /// <summary>
    /// UTC timestamp when the entity was created
    /// </summary>
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// UTC timestamp when the entity was last updated
    /// </summary>
    public DateTime? UpdatedAt { get; set; }
}
