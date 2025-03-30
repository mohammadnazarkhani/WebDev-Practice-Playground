using Microsoft.EntityFrameworkCore;
using EFCoreShowcase.Common.Pagination;

namespace EFCoreShowcase.Common.Extensions;

public static class QueryableExtensions
{
    public static async Task<PaginatedResult<T>> ToPaginatedListAsync<T>(
        this IQueryable<T> source,
        int pageNumber,
        int pageSize,
        CancellationToken cancellationToken = default)
    {
        var count = await source.CountAsync(cancellationToken);
        var items = await source
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        return new PaginatedResult<T>(items, count, pageNumber, pageSize);
    }

    public static IQueryable<T> WithNoTracking<T>(this IQueryable<T> source) where T : class
        => source.AsNoTracking();

    public static IQueryable<T> IncludeMultiple<T>(
        this IQueryable<T> source,
        params string[] includeProperties) where T : class
        => includeProperties.Aggregate(source, (current, include) => current.Include(include));
}
