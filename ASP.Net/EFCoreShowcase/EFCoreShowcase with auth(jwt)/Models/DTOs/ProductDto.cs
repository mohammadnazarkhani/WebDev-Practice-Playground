namespace EFCoreShowcase.Models.DTOs
{
    public record ProductDto(int Id, string Name, decimal Price, int CategoryId, string UserId);
    public record CreateProductDto(string Name, decimal Price, int CategoryId);
    public record UpdateProductDto(string Name, decimal Price, int CategoryId);
}
