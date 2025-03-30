using EFCoreShowcase.Core.Interfaces;
using EFCoreShowcase.Data;
using Microsoft.EntityFrameworkCore;

namespace EFCoreShowcase.Infrastructure.Validation;

public class DatabaseCategoryValidator : ICategoryValidator
{
    private readonly AppDbContext _context;

    public DatabaseCategoryValidator(AppDbContext context)
    {
        _context = context;
    }

    public async Task<bool> CategoryExistsAsync(int categoryId, CancellationToken cancellation = default)
        => await _context.Categories.AnyAsync(c => c.Id == categoryId, cancellation);

    public async Task<bool> IsNameUniqueAsync(string name, CancellationToken cancellation = default)
        => !await _context.Categories.AnyAsync(c => c.Name == name, cancellation);

    public bool CategoryExists(int categoryId)
        => _context.Categories.Any(c => c.Id == categoryId);

    public bool IsNameUnique(string name)
        => !_context.Categories.Any(c => c.Name == name);
}
