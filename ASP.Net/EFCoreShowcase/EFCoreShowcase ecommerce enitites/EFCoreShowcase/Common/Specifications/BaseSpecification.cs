using System.Linq.Expressions;

namespace EFCoreShowcase.Common.Specifications;

public class BaseSpecification<T> : ISpecification<T>
{
    Expression<Func<T, bool>>? ISpecification<T>.Criteria => Criteria;
    List<Expression<Func<T, object>>> ISpecification<T>.Includes => Includes;
    Expression<Func<T, object>>? ISpecification<T>.OrderBy => OrderBy;
    Expression<Func<T, object>>? ISpecification<T>.OrderByDescending => OrderByDescending;

    protected Expression<Func<T, bool>>? Criteria { get; private set; }
    protected List<Expression<Func<T, object>>> Includes { get; } = new();
    protected Expression<Func<T, object>>? OrderBy { get; private set; }
    protected Expression<Func<T, object>>? OrderByDescending { get; private set; }

    public int Take { get; private set; }
    public int Skip { get; private set; }
    public bool IsPagingEnabled { get; private set; }

    protected void AddInclude(Expression<Func<T, object>> includeExpression)
        => Includes.Add(includeExpression);

    protected void AddOrderBy(Expression<Func<T, object>> orderByExpression)
        => OrderBy = orderByExpression;

    protected void AddOrderByDescending(Expression<Func<T, object>> orderByDescExpression)
        => OrderByDescending = orderByDescExpression;

    protected void ApplyPaging(int skip, int take)
    {
        Skip = skip;
        Take = take;
        IsPagingEnabled = true;
    }

    protected void ApplyCriteria(Expression<Func<T, bool>> criteria)
        => Criteria = criteria;
}
