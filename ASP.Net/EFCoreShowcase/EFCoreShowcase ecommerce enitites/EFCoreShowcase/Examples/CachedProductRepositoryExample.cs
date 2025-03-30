using EFCoreShowcase.Common.Repository;
using EFCoreShowcase.Common.Caching;
using EFCoreShowcase.Common.Specifications;
using EFCoreShowcase.Entities;
using EFCoreShowcase.DTOs;

namespace EFCoreShowcase.Examples;

public class CachedProductRepositoryExample
{
    private readonly IRepository<Product> _productRepository;
    private readonly ICacheService _cacheService;
    private const int DEFAULT_CACHE_DURATION_MINUTES = 30;

    public CachedProductRepositoryExample(
        IRepository<Product> productRepository,
        ICacheService cacheService)
    {
        _productRepository = productRepository;
        _cacheService = cacheService;
    }

    public async Task<Product?> GetProductByIdAsync(long id)
    {
        var cacheKey = $"product:{id}";

        // Try to get from cache first
        var cachedProduct = await _cacheService.GetAsync<Product>(cacheKey);
        if (cachedProduct != null)
            return cachedProduct;

        // If not in cache, get from repository
        var product = await _productRepository.GetByIdAsync(id);
        if (product != null)
        {
            // Cache the result for 30 minutes
            await _cacheService.SetAsync(cacheKey, product, TimeSpan.FromMinutes(DEFAULT_CACHE_DURATION_MINUTES));
        }

        return product;
    }

    public async Task<IReadOnlyList<ProductListDto>> GetFeaturedProductsAsync()
    {
        var cacheKey = "featured-products";

        // Try to get from cache
        var cachedProducts = await _cacheService.GetAsync<List<ProductListDto>>(cacheKey);
        if (cachedProducts != null)
            return cachedProducts;

        // If not in cache, get from repository
        var spec = new SpecificationBuilder<Product>()
            .Where(p => p.IsActive)  // Removed IsFeatured condition
            .OrderBy(p => p.Name);

        var products = await _productRepository.ListWithSpecAsync(spec);

        // Map to DTOs
        var productDtos = products.Select(p => new ProductListDto
        {
            Id = p.Id,
            Name = p.Name,
            Price = p.Price,
            MainImageUrl = p.MainImage?.ImageUrl,
            AverageRating = p.AverageRating,
            ReviewCount = p.Reviews.Count
        }).ToList();

        // Cache the result for 1 hour
        await _cacheService.SetAsync(cacheKey, productDtos, TimeSpan.FromHours(1));

        return productDtos;
    }

    public async Task<Dictionary<string, decimal>> GetCachedAveragePriceByCategoryAsync()
    {
        var cacheKey = "category-avg-prices";

        // Try to get from cache
        var cachedAverages = await _cacheService.GetAsync<Dictionary<string, decimal>>(cacheKey);
        if (cachedAverages != null)
            return cachedAverages;

        // If not in cache, calculate
        var result = await _productRepository.GetGroupedAsync(
            p => new { p.Category.Name, p.Price.Amount },
            x => x.Name,
            x => x.Name,
            g => new { Category = g.Key, AveragePrice = g.Average(x => x.Amount) }
        );

        var averages = result.ToDictionary(x => x.Category, x => x.AveragePrice);

        // Cache the result for 12 hours
        await _cacheService.SetAsync(cacheKey, averages, TimeSpan.FromHours(12));

        return averages;
    }

    public async Task InvalidateProductCacheAsync(long productId)
    {
        // Remove specific product cache
        await _cacheService.RemoveAsync($"product:{productId}");

        // Remove related caches that might contain this product
        await _cacheService.RemoveAsync("featured-products");
        await _cacheService.RemoveAsync("category-avg-prices");
    }

    public async Task<IReadOnlyList<ProductListDto>> GetProductsByCategoryAsync(long categoryId)
    {
        var cacheKey = $"category:{categoryId}:products";

        // Try batch get from cache with multiple keys
        var keys = new[] { cacheKey, "category-last-updated" };
        var cachedData = await _cacheService.GetAllAsync<object>(keys);

        // Check if cache is still valid
        if (cachedData.TryGetValue(cacheKey, out var cached) &&
            cachedData.TryGetValue("category-last-updated", out var lastUpdated))
        {
            if (cached is List<ProductListDto> cachedProducts)
                return cachedProducts;
        }

        // If not in cache or invalid, get from repository
        var spec = new SpecificationBuilder<Product>()
            .Where(p => p.CategoryId == categoryId && p.IsActive)
            .OrderBy(p => p.Name);

        var products = await _productRepository.ListWithSpecAsync(spec);

        var productDtos = products.Select(p => new ProductListDto
        {
            Id = p.Id,
            Name = p.Name,
            Price = p.Price,
            MainImageUrl = p.MainImage?.ImageUrl,
            AverageRating = p.AverageRating,
            ReviewCount = p.Reviews.Count
        }).ToList();

        // Cache with sliding expiration
        var cacheEntries = new Dictionary<string, object>
        {
            { cacheKey, productDtos },
            { "category-last-updated", DateTime.UtcNow }
        };

        await _cacheService.SetAllAsync(cacheEntries, TimeSpan.FromHours(2));

        return productDtos;
    }
}
