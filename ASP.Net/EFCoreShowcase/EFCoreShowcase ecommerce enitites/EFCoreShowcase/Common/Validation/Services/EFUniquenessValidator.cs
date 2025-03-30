using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using EFCoreShowcase.Data;

namespace EFCoreShowcase.Common.Validation.Services;

public class EFUniquenessValidator : IUniquenessValidator
{
    private readonly AppDbContext _dbContext;

    public EFUniquenessValidator(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<bool> IsUniqueAsync<TEntity>(string propertyName, string value, long? excludeId = null) where TEntity : class
    {
        var query = _dbContext.Set<TEntity>().AsQueryable();
        var parameter = Expression.Parameter(typeof(TEntity), "x");
        var property = Expression.Property(parameter, propertyName);
        var constant = Expression.Constant(value);
        var equals = Expression.Equal(property, constant);
        var lambda = Expression.Lambda<Func<TEntity, bool>>(equals, parameter);

        if (excludeId.HasValue)
        {
            var idProperty = Expression.Property(parameter, "Id");
            var idConstant = Expression.Constant(excludeId.Value);
            var notEquals = Expression.NotEqual(idProperty, idConstant);
            var andExpression = Expression.AndAlso(equals, notEquals);
            lambda = Expression.Lambda<Func<TEntity, bool>>(andExpression, parameter);
        }

        return !await query.AnyAsync(lambda);
    }

    public bool IsUnique<TEntity>(string propertyName, string value, long? excludeId = null) where TEntity : class
    {
        return IsUniqueAsync<TEntity>(propertyName, value, excludeId).GetAwaiter().GetResult();
    }
}
