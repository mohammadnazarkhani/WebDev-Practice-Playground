using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using EFCoreShowcase.Entities.Interfaces;
using EFCoreShowcase.Entities.ValueObjects;
using EFCoreShowcase.Entities.Base;

public class Product : AuditableEntity, IVersionable
{
    /// <summary>
    /// Domain events collection for tracking changes and triggering side effects.
    /// </summary>
    private readonly List<DomainEvent> _domainEvents = new();
    public IReadOnlyCollection<DomainEvent> DomainEvents => _domainEvents.AsReadOnly();

    [Required, StringLength(200)]
    public required string Name { get; set; }

    [Required, StringLength(2000)]
    public required string Description { get; set; }

    [DataType(DataType.Currency)]
    [Column(TypeName = "decimal(18,2)")]
    public Money Price { get; set; } = null!;

    [Range(0, int.MaxValue)]
    public int StockQuantity { get; set; }

    /// <summary>
    /// Unique stock keeping unit. Format: XX-000000
    /// </summary>
    [RegularExpression(@"^[A-Z]{2}-\d{6}$")]
    public required string SKU { get; set; }

    public bool IsActive { get; set; }

    /// <summary>
    /// Relationship: Many-to-One with Category
    /// Foreign Key: CategoryId
    /// </summary>
    public long CategoryId { get; set; }
    public Category Category { get; set; } = null!;

    /// <summary>
    /// Relationship: One-to-Many with OrderItem
    /// Used for order history and inventory tracking
    /// </summary>
    public ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();

    /// <summary>
    /// Relationship: One-to-Many with ProductImage
    /// Includes a designated main image for primary display
    /// </summary>
    public ICollection<ProductImage> Images { get; set; } = new List<ProductImage>();
    public Guid? MainImageId { get; set; }
    public ProductImage? MainImage { get; set; }

    public ICollection<ProductVideo> Videos { get; set; } = new List<ProductVideo>();
    public Guid? MainVideoId { get; set; }
    public ProductVideo? MainVideo { get; set; }

    public ICollection<ProductSpecification> Specifications { get; set; } = new List<ProductSpecification>();
    public ICollection<ProductReview> Reviews { get; set; } = new List<ProductReview>();
    public ICollection<ProductQuestion> Questions { get; set; } = new List<ProductQuestion>();

    [Range(0, 5)]
    public decimal AverageRating { get; set; }
    public int ReviewCount { get; set; }

    [Timestamp]
    public byte[] RowVersion { get; set; } = null!;

    /// <summary>
    /// Computed column based on stock threshold
    /// True when StockQuantity <= MinStockThreshold
    /// </summary>
    [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
    public bool IsLowStock { get; private set; }

    public int MinStockThreshold { get; set; } = 10;

    public void AddDomainEvent(DomainEvent domainEvent)
    {
        _domainEvents.Add(domainEvent);
    }

    public void ClearDomainEvents()
    {
        _domainEvents.Clear();
    }

    public EntityValidationResult Validate()
    {
        var errors = new List<string>();

        if (StockQuantity < 0)
            errors.Add("Stock quantity cannot be negative");
        if (Price.Amount < 0)
            errors.Add("Price cannot be negative");
        if (string.IsNullOrWhiteSpace(SKU))
            errors.Add("SKU is required");

        return errors.Any()
            ? EntityValidationResult.Failure(errors)
            : EntityValidationResult.Success();
    }
}
