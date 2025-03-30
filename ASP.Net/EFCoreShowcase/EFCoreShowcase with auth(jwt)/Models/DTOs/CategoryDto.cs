namespace EFCoreShowcase.Models.DTOs
{
    public record CategoryDto(int Id, string Name);
    public record CreateCategoryDto(string Name);
    public record UpdateCategoryDto(string Name);
}
