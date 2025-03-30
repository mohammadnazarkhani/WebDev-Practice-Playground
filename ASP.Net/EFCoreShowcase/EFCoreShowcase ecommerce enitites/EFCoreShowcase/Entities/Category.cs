public class Category : AuditableEntity
{
    public required string Name { get; set; }
    public required string Description { get; set; }

    public long? ParentCategoryId { get; set; }
    public Category? ParentCategory { get; set; }
    public ICollection<Category> SubCategories { get; set; } = new List<Category>();
    public ICollection<Product> Products { get; set; } = new List<Product>();
}
