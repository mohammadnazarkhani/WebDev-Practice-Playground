using System.Linq.Expressions;
using EFCoreShowcase.Common.Repository;

namespace EFCoreShowcase.Common.Extensions;

public static class SortingExtension
{
    public static IQueryable<T> GetOrdering<T>(IQueryable<T> query, List<SortingParam> sortingParams)
    {
        if (!sortingParams.Any()) return query;

        var parameter = Expression.Parameter(typeof(T), "x");
        var first = sortingParams[0];
        var property = Expression.Property(parameter, first.PropertyName);
        var lambda = Expression.Lambda(Expression.Convert(property, typeof(object)), parameter);

        query = first.IsDescending
            ? Queryable.OrderByDescending(query, (dynamic)lambda)
            : Queryable.OrderBy(query, (dynamic)lambda);

        for (int i = 1; i < sortingParams.Count; i++)
        {
            var param = sortingParams[i];
            property = Expression.Property(parameter, param.PropertyName);
            lambda = Expression.Lambda(Expression.Convert(property, typeof(object)), parameter);

            query = param.IsDescending
                ? Queryable.ThenByDescending(query, (dynamic)lambda)
                : Queryable.ThenBy(query, (dynamic)lambda);
        }

        return query;
    }
}
