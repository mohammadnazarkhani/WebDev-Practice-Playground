namespace EFCoreShowcase.Common.Repository;

public class QueryResult<T>
{
    public IEnumerable<T> Items { get; }
    public int TotalCount { get; }

    public QueryResult(IEnumerable<T> items, int totalCount)
    {
        Items = items;
        TotalCount = totalCount;
    }
}
