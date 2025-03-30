using EFCoreShowcase.Common.Specifications;
using EFCoreShowcase.Entities;
using EFCoreShowcase.Common.Repository;
using System.Linq.Expressions;

namespace EFCoreShowcase.Examples;

public class ProductSearchSpecification : BaseSpecification<Product>
{
    public ProductSearchSpecification(
        string? searchTerm = null,
        decimal? minPrice = null,
        decimal? maxPrice = null,
        long? categoryId = null,
        int? minRating = null,
        int pageSize = 10,
        int pageNumber = 1)
    {
        // Base query
        if (!string.IsNullOrEmpty(searchTerm))
        {
            ApplyCriteria(p => p.Name.Contains(searchTerm) ||
                              p.Description.Contains(searchTerm));
        }

        // Price filter
        if (minPrice.HasValue)
            ApplyCriteria(p => p.Price.Amount >= minPrice.Value);

        if (maxPrice.HasValue)
            ApplyCriteria(p => p.Price.Amount <= maxPrice.Value);

        // Category filter
        if (categoryId.HasValue)
            ApplyCriteria(p => p.CategoryId == categoryId.Value);

        // Rating filter
        if (minRating.HasValue)
            ApplyCriteria(p => p.AverageRating >= minRating.Value);

        // Initialize includes - Changed to use non-generic AddInclude
        AddInclude(p => p.Category);
        AddInclude(p => p.MainImage!);
        AddInclude(p => p.Specifications);
        AddInclude(p => p.Reviews);

        // Add ordering
        AddOrderByDescending(p => p.CreatedAt);

        // Add paging
        ApplyPaging((pageNumber - 1) * pageSize, pageSize);
    }
}

public static class SpecificationExtensions
{
    public static void AddInclude<T>(this BaseSpecification<T> spec, Expression<Func<T, object>> includeExpression) where T : class
    {
        spec.GetType()
            .GetMethod("AddInclude", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)!
            .Invoke(spec, new object[] { includeExpression });
    }
}

// Usage Example
public class ProductSearchService
{
    private readonly IRepository<Product> _productRepository;

    public ProductSearchService(IRepository<Product> productRepository)
    {
        _productRepository = productRepository;
    }

    public async Task<IReadOnlyList<Product>> SearchProductsAsync(
        string searchTerm,
        decimal? minPrice,
        decimal? maxPrice,
        long? categoryId,
        int? minRating,
        int pageSize = 10,
        int pageNumber = 1)
    {
        var spec = new ProductSearchSpecification(
            searchTerm, minPrice, maxPrice, categoryId, minRating, pageSize, pageNumber);

        var results = await _productRepository.ListWithSpecAsync(spec);
        // Changed to initialize empty list if null
        return results ?? Array.Empty<Product>().ToList();
    }
}
