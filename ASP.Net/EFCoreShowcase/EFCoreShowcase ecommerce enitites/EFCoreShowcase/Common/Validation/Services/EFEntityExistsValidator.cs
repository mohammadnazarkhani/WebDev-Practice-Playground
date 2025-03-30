using Microsoft.EntityFrameworkCore;
using EFCoreShowcase.Data;

namespace EFCoreShowcase.Common.Validation.Services;

public class EFEntityExistsValidator : IEntityExistsValidator
{
    private readonly AppDbContext _dbContext;

    public EFEntityExistsValidator(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<bool> ExistsAsync<TEntity>(long id) where TEntity : class
    {
        return await _dbContext.Set<TEntity>().FindAsync(id) != null;
    }

    public bool Exists<TEntity>(long id) where TEntity : class
    {
        return ExistsAsync<TEntity>(id).GetAwaiter().GetResult();
    }
}
