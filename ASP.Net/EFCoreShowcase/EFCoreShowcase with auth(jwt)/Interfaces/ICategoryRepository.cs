using EFCoreShowcase.Models.DTOs;

namespace EFCoreShowcase.Interfaces
{
    public interface ICategoryRepository
    {
        Task<IEnumerable<CategoryDto>> GetAllAsync(CancellationToken cancellationToken);
        Task<CategoryDto> CreateAsync(CreateCategoryDto categoryDto, CancellationToken cancellationToken);
        Task<bool> DeleteAsync(int id, CancellationToken cancellationToken);
    }
}
