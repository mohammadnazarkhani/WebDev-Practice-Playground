public class ProductSpecification : AuditableEntity
{
    public required string SpecificationKey { get; set; }
    public required string SpecificationValue { get; set; }
    public int DisplayOrder { get; set; }
    public required string Group { get; set; }

    public long ProductId { get; set; }
    public required Product Product { get; set; }
}
