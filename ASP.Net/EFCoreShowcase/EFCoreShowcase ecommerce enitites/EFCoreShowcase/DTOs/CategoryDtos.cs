using System.ComponentModel.DataAnnotations;

namespace EFCoreShowcase.DTOs;

public record CategoryDto
{
    public long Id { get; init; }
    public string Name { get; init; } = null!;
    public string Description { get; init; } = null!;
    public long? ParentCategoryId { get; init; }
    public ICollection<CategoryDto> SubCategories { get; init; } = new List<CategoryDto>();
}

public record CreateCategoryDto
{
    [Required, StringLength(100)]
    public string Name { get; init; } = null!;

    [Required, StringLength(1000)]
    public string Description { get; init; } = null!;

    public long? ParentCategoryId { get; init; }
}

public record UpdateCategoryDto
{
    [StringLength(100)]
    public string? Name { get; init; }

    [StringLength(1000)]
    public string? Description { get; init; }

    public long? ParentCategoryId { get; init; }
}

public record CategoryTreeDto : CategoryDto
{
    public int Level { get; init; }
    public string FullPath { get; init; } = null!;
}
