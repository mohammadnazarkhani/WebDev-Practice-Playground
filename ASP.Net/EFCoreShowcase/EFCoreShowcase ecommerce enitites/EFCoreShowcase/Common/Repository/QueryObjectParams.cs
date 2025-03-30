namespace EFCoreShowcase.Common.Repository;

public class QueryObjectParams
{
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 10;
    public string? SortBy { get; set; }
    public bool IsSortAscending { get; set; } = true;
    public string? SearchTerm { get; set; }
    public Dictionary<string, string> Filters { get; set; } = new();
    public List<SortingParam> SortingParams { get; set; } = new();
    public int PageNumber { get; set; } = 1;
    public bool IncludeDeleted { get; set; }
}

public class SortingParam
{
    public required string PropertyName { get; set; }
    public bool IsDescending { get; set; }
}
