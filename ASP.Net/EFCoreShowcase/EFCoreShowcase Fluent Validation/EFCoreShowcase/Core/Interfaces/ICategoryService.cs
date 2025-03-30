using EFCoreShowcase.DTOs;
using EFCoreShowcase.Models;

namespace EFCoreShowcase.Core.Interfaces;

public interface ICategoryService
{
    Task<IEnumerable<Category>> GetCategories();
    Task<Category> CreateCategory(CategoryDto categoryDto);
    Task<Category?> GetCategory(int id);
    Task<Category> UpdateCategory(int id, CategoryDto categoryDto);
    Task DeleteCategory(int id);
}
