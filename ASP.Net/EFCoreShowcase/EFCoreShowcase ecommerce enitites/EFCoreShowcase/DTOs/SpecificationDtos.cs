using System.ComponentModel.DataAnnotations;

namespace EFCoreShowcase.DTOs;

public record ProductSpecificationDto
{
    public long Id { get; init; }
    public string SpecificationKey { get; init; } = null!;
    public string SpecificationValue { get; init; } = null!;
    public string Group { get; init; } = null!;
    public int DisplayOrder { get; init; }
}

public record CreateSpecificationDto
{
    [Required, StringLength(100)]
    public string SpecificationKey { get; init; } = null!;

    [Required, StringLength(500)]
    public string SpecificationValue { get; init; } = null!;

    [Required, StringLength(100)]
    public string Group { get; init; } = null!;

    public int DisplayOrder { get; init; }
}
