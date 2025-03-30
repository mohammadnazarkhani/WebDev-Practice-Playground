using EFCoreShowcase.Core.Interfaces;
using EFCoreShowcase.Data;
using EFCoreShowcase.DTOs;
using EFCoreShowcase.Models;
using Microsoft.EntityFrameworkCore;

namespace EFCoreShowcase.Infrastructure.Services;

public class CategoryService : ICategoryService
{
    private readonly AppDbContext _context;

    public CategoryService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Category>> GetCategories()
        => await _context.Categories.ToListAsync();

    public async Task<Category> CreateCategory(CategoryDto categoryDto)
    {
        var category = new Category { Name = categoryDto.Name };
        _context.Categories.Add(category);
        await _context.SaveChangesAsync();
        return category;
    }

    public async Task<Category?> GetCategory(int id)
        => await _context.Categories.FindAsync(id);

    public async Task<Category> UpdateCategory(int id, CategoryDto categoryDto)
    {
        var category = await _context.Categories.FindAsync(id);
        if (category == null)
            throw new KeyNotFoundException($"Category with ID {id} not found.");

        category.Name = categoryDto.Name;
        await _context.SaveChangesAsync();
        return category;
    }

    public async Task DeleteCategory(int id)
    {
        var category = await _context.Categories.FindAsync(id);
        if (category == null)
            throw new KeyNotFoundException($"Category with ID {id} not found.");

        _context.Categories.Remove(category);
        await _context.SaveChangesAsync();
    }
}
