using System.Linq.Expressions;

namespace EFCoreShowcase.Common.Specifications;

public class SpecificationBuilder<T> : BaseSpecification<T>
{
    public SpecificationBuilder<T> Where(Expression<Func<T, bool>> criteria)
    {
        ApplyCriteria(criteria);
        return this;
    }

    public SpecificationBuilder<T> Include(Expression<Func<T, object>> includeExpression)
    {
        AddInclude(includeExpression);
        return this;
    }

    public new SpecificationBuilder<T> OrderBy(Expression<Func<T, object>> orderByExpression)
    {
        AddOrderBy(orderByExpression);
        return this;
    }

    public new SpecificationBuilder<T> OrderByDescending(Expression<Func<T, object>> orderByDescExpression)
    {
        AddOrderByDescending(orderByDescExpression);
        return this;
    }

    public SpecificationBuilder<T> WithPaging(int pageNumber, int pageSize)
    {
        ApplyPaging((pageNumber - 1) * pageSize, pageSize);
        return this;
    }
}
