using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using EFCoreShowcase.Common.Specifications;
using EFCoreShowcase.Data;
using EFCoreShowcase.Common.Pagination;

namespace EFCoreShowcase.Common.Repository;

public class GenericRepository<T> : IRepository<T> where T : class
{
    private readonly AppDbContext _context;
    private readonly DbSet<T> _dbSet;

    public GenericRepository(AppDbContext context)
    {
        _context = context;
        _dbSet = context.Set<T>();
    }

    public async Task<T?> GetByIdAsync(long id)
        => await _dbSet.FindAsync(id);

    public async Task<IReadOnlyList<T>> ListAllAsync()
        => await _dbSet.ToListAsync();

    public async Task<T?> GetEntityWithSpec(ISpecification<T> spec)
        => await ApplySpecification(spec).FirstOrDefaultAsync();

    public async Task<IReadOnlyList<T>> ListWithSpecAsync(ISpecification<T> spec)
        => await ApplySpecification(spec).ToListAsync();

    public async Task<int> CountAsync(ISpecification<T> spec)
        => await ApplySpecification(spec).CountAsync();

    public void Add(T entity) => _dbSet.Add(entity);
    public void Update(T entity) => _context.Entry(entity).State = EntityState.Modified;
    public void Delete(T entity) => _dbSet.Remove(entity);

    public async Task<List<int>> AddRangeAsync(List<T> entities)
    {
        await _dbSet.AddRangeAsync(entities);
        return entities.Select(e => (int)e.GetType().GetProperty("Id")?.GetValue(e)!).ToList();
    }

    public async Task<IEnumerable<TResult>> SelectAsync<TResult>(Expression<Func<T, TResult>> selector)
    {
        return await _dbSet.Select(selector).ToListAsync();
    }

    public async Task<IEnumerable<TResult>> SelectAsync<TResult>(
        Expression<Func<T, TResult>> selector,
        Expression<Func<T, bool>>? predicate = null)
    {
        var query = _dbSet.AsQueryable();
        if (predicate != null)
            query = query.Where(predicate);
        return await query.Select(selector).ToListAsync();
    }

    public async Task<QueryResult<T>> GetPageAsync(
        QueryObjectParams queryParams,
        Expression<Func<T, bool>>? predicate = null,
        List<Expression<Func<T, object>>>? includes = null)
    {
        var query = _dbSet.AsQueryable();

        if (predicate != null)
            query = query.Where(predicate);

        if (includes != null)
            query = includes.Aggregate(query, (current, include) => current.Include(include));

        var totalItems = await query.CountAsync();

        if (!string.IsNullOrWhiteSpace(queryParams.SortBy))
        {
            var parameter = Expression.Parameter(typeof(T), "x");
            var property = Expression.Property(parameter, queryParams.SortBy);
            var lambda = Expression.Lambda<Func<T, object>>(Expression.Convert(property, typeof(object)), parameter);

            query = queryParams.IsSortAscending
                ? query.OrderBy(lambda)
                : query.OrderByDescending(lambda);
        }

        query = query.Skip((queryParams.Page - 1) * queryParams.PageSize)
                    .Take(queryParams.PageSize);

        var items = await query.ToListAsync();

        return new QueryResult<T>(items, totalItems);
    }

    public async Task<int> UpdateRangeAsync(List<T> entities)
    {
        _dbSet.UpdateRange(entities);
        return await _context.SaveChangesAsync();
    }

    public async Task<int> DeleteRangeAsync(IEnumerable<T> entities)
    {
        _dbSet.RemoveRange(entities);
        return await _context.SaveChangesAsync();
    }

    public async Task<int> DeleteByPredicateAsync(Expression<Func<T, bool>> predicate)
    {
        var entities = await _dbSet.Where(predicate).ToListAsync();
        if (!entities.Any()) return 0;

        _dbSet.RemoveRange(entities);
        return await _context.SaveChangesAsync();
    }

    public async Task<int> UpdateAsync(T entityToUpdate)
    {
        _context.Entry(entityToUpdate).State = EntityState.Modified;
        return await _context.SaveChangesAsync();
    }

    public async Task<int> CountAllAsync()
        => await _dbSet.CountAsync();

    public async Task<int> CountAsync(Expression<Func<T, bool>> predicate)
        => await _dbSet.CountAsync(predicate);

    public async Task<T?> FirstOrDefaultAsync(Expression<Func<T, bool>> predicate)
        => await _dbSet.FirstOrDefaultAsync(predicate);

    public async Task<T?> LastOrDefaultAsync()
        => await _dbSet.OrderByDescending(e => EF.Property<object>(e, "Id")).FirstOrDefaultAsync();

    public async Task<IEnumerable<TResult>> GetAllSelect<TResult>(
        Expression<Func<T, TResult>> selector,
        Expression<Func<T, bool>> predicate)
        => await _dbSet.Where(predicate).Select(selector).ToListAsync();

    public async Task<IEnumerable<T>> GetAllWithIncludeAsync<TProperty>(
        Expression<Func<T, TProperty>> include,
        Expression<Func<T, bool>>? predicate = null)
    {
        IIncludableQueryable<T, TProperty> query = _dbSet.Include(include);
        if (predicate != null)
            query = (IIncludableQueryable<T, TProperty>)query.Where(predicate);
        return await query.ToListAsync();
    }

    public async Task<IEnumerable<T>> GetAllPaginatedAsync(int page, int take)
    {
        int skip = (take * page) - take;
        return await _dbSet
            .Skip(skip)
            .Take(take)
            .ToListAsync();
    }

    /// <summary>
    /// Executes complex grouped queries with sorting, filtering, and custom projections.
    /// </summary>
    /// <typeparam name="TResult">Intermediate result type for initial projection</typeparam>
    /// <typeparam name="TKey">Type used for ordering the results</typeparam>
    /// <typeparam name="TGroup">Type used for grouping the results</typeparam>
    /// <typeparam name="TReturn">Final result type after grouping transformation</typeparam>
    /// <param name="selector">Initial projection expression</param>
    /// <param name="orderBy">Expression for ordering results</param>
    /// <param name="groupBy">Function to group results</param>
    /// <param name="resultSelector">Transformation function for grouped results</param>
    /// <returns>Collection of transformed grouped results</returns>
    /// <example>
    /// <code>
    /// var stats = await repository.GetGroupedAsync(
    ///     p => new { p.Category, p.Price },                    // Select initial fields
    ///     x => x.Category.Name,                               // Order by category name
    ///     x => x.Category,                                    // Group by category
    ///     g => new { Category = g.Key, Average = g.Average(x => x.Price) } // Transform groups
    /// );
    /// </code>
    /// </example>
    public async Task<IEnumerable<TReturn>> GetGroupedAsync<TResult, TKey, TGroup, TReturn>(
        Expression<Func<T, TResult>> selector,
        Expression<Func<TResult, TKey>> orderBy,
        Func<TResult, TGroup> groupBy,
        Func<IGrouping<TGroup, TResult>, TReturn> resultSelector)
    {
        // First materialize the initial query results
        var results = await _dbSet
            .Select(selector)
            .OrderBy(orderBy)
            .ToListAsync();

        // Then perform grouping and final selection in memory
        return results
            .GroupBy(groupBy)
            .Select(g => resultSelector(g))
            .ToList();
    }

    /// <summary>
    /// Applies a specification pattern to build a query with includes, filters, and ordering.
    /// </summary>
    /// <param name="spec">The specification containing query criteria and includes</param>
    /// <returns>IQueryable with applied specification</returns>
    /// <remarks>
    /// This method builds a query that:
    /// 1. Applies WHERE conditions from the specification
    /// 2. Includes related entities
    /// 3. Applies ordering
    /// 4. Handles paging if enabled
    /// </remarks>
    private IQueryable<T> ApplySpecification(ISpecification<T> spec)
    {
        var query = _dbSet.AsQueryable();

        if (spec.Criteria != null)
            query = query.Where(spec.Criteria);

        query = spec.Includes.Aggregate(query, (current, include) => current.Include(include));

        if (spec.OrderBy != null)
            query = query.OrderBy(spec.OrderBy);
        else if (spec.OrderByDescending != null)
            query = query.OrderByDescending(spec.OrderByDescending);

        if (spec.IsPagingEnabled)
            query = query.Skip(spec.Skip).Take(spec.Take);

        return query;
    }
}
