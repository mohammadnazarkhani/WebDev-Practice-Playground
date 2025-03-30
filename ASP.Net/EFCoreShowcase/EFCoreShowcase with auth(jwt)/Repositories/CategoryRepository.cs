using EFCoreShowcase.Data;
using EFCoreShowcase.Interfaces;
using EFCoreShowcase.Models;
using EFCoreShowcase.Models.DTOs;
using Microsoft.EntityFrameworkCore;

namespace EFCoreShowcase.Repositories
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly AppDbContext _context;

        public CategoryRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<CategoryDto>> GetAllAsync(CancellationToken cancellationToken)
        {
            return await _context.Categories
                .Select(c => new CategoryDto(c.Id, c.Name))
                .ToListAsync(cancellationToken);
        }

        public async Task<CategoryDto> CreateAsync(CreateCategoryDto categoryDto, CancellationToken cancellationToken)
        {
            var category = new Category
            {
                Name = categoryDto.Name
            };

            _context.Categories.Add(category);
            await _context.SaveChangesAsync(cancellationToken);

            return new CategoryDto(category.Id, category.Name);
        }

        public async Task<bool> DeleteAsync(int id, CancellationToken cancellationToken)
        {
            var category = await _context.Categories
                .Include(c => c.Products)  // Include related products
                .FirstOrDefaultAsync(c => c.Id == id, cancellationToken);

            if (category == null) return false;

            // Remove all products first
            if (category.Products.Any())
            {
                _context.Products.RemoveRange(category.Products);
            }

            // Then remove the category
            _context.Categories.Remove(category);
            await _context.SaveChangesAsync(cancellationToken);
            return true;
        }
    }
}
