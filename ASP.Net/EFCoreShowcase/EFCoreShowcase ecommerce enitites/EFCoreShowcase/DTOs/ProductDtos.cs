using System.ComponentModel.DataAnnotations;
using EFCoreShowcase.Entities.ValueObjects;

namespace EFCoreShowcase.DTOs;

public record ProductDto
{
    public long Id { get; init; }
    public string Name { get; init; } = null!;
    public string Description { get; init; } = null!;
    public Money Price { get; init; } = null!;
    public string SKU { get; init; } = null!;
    public int StockQuantity { get; init; }
    public bool IsActive { get; init; }
    public decimal AverageRating { get; init; }
    public int ReviewCount { get; init; }
    public string CategoryName { get; init; } = null!;
    public DateTime CreatedAt { get; init; }
}

// Add a detailed product DTO for full product information
public record ProductDetailDto
{
    public long Id { get; init; }
    public string Name { get; init; } = null!;
    public string Description { get; init; } = null!;
    public Money Price { get; init; } = null!;
    public string SKU { get; init; } = null!;
    public int StockQuantity { get; init; }
    public bool IsActive { get; init; }
    public decimal AverageRating { get; init; }
    public int ReviewCount { get; init; }
    public string CategoryName { get; init; } = null!;
    public DateTime CreatedAt { get; init; }
    public IReadOnlyCollection<ProductSpecificationDto> Specifications { get; init; } = Array.Empty<ProductSpecificationDto>();
    public IReadOnlyCollection<ProductReviewDto> Reviews { get; init; } = Array.Empty<ProductReviewDto>();
    public IReadOnlyCollection<ProductImageDto> Images { get; init; } = Array.Empty<ProductImageDto>();
    public ProductImageDto? MainImage { get; init; }
    public ProductVideoDto? MainVideo { get; init; }
}

// Add minimal DTO for list views
public record ProductListDto
{
    public long Id { get; init; }
    public string Name { get; init; } = null!;
    public Money Price { get; init; } = null!;
    public string? MainImageUrl { get; init; }
    public decimal AverageRating { get; init; }
    public int ReviewCount { get; init; }
}

public record CreateProductDto
{
    [Required, StringLength(200)]
    public string Name { get; init; } = null!;

    [Required, StringLength(2000)]
    public string Description { get; init; } = null!;

    [Required]
    public decimal Price { get; init; }

    [Required]
    public string Currency { get; init; } = null!;

    [Required, RegularExpression(@"^[A-Z]{2}-\d{6}$")]
    public string SKU { get; init; } = null!;

    [Required]
    public long CategoryId { get; init; }

    [Range(0, int.MaxValue)]
    public int StockQuantity { get; init; }
}

public record UpdateProductDto
{
    [StringLength(200)]
    public string? Name { get; init; }

    [StringLength(2000)]
    public string? Description { get; init; }

    public decimal? Price { get; init; }
    public string? Currency { get; init; }

    [Range(0, int.MaxValue)]
    public int? StockQuantity { get; init; }
    public bool? IsActive { get; init; }
}
