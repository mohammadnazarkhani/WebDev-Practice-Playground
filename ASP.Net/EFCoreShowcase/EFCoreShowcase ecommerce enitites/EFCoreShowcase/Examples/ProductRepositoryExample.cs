using EFCoreShowcase.Common.Repository;
using EFCoreShowcase.Common.Specifications;
using EFCoreShowcase.Entities;
using EFCoreShowcase.DTOs;

namespace EFCoreShowcase.Examples;

public class ProductRepositoryExample
{
    private readonly IRepository<Product> _productRepository;

    public ProductRepositoryExample(IRepository<Product> productRepository)
    {
        _productRepository = productRepository;
    }

    public async Task<Product?> GetProductByIdAsync(long id)
    {
        return await _productRepository.GetByIdAsync(id);
    }

    public async Task<IReadOnlyList<Product>> GetActiveProductsAsync()
    {
        var spec = new SpecificationBuilder<Product>()
            .Where(p => p.IsActive)
            .OrderBy(p => p.Name);

        return await _productRepository.ListWithSpecAsync(spec);
    }

    public async Task<IEnumerable<ProductListDto>> GetProductsByPriceRangeAsync(decimal minPrice, decimal maxPrice)
    {
        return await _productRepository.SelectAsync(
            p => new ProductListDto
            {
                Id = p.Id,
                Name = p.Name,
                Price = p.Price,
                MainImageUrl = p.MainImage != null ? p.MainImage.ImageUrl : null,
                AverageRating = p.AverageRating,
                ReviewCount = p.Reviews.Count
            },
            p => p.Price.Amount >= minPrice && p.Price.Amount <= maxPrice
        );
    }

    public async Task<Dictionary<string, decimal>> GetAveragePriceByCategoryAsync()
    {
        var result = await _productRepository.GetGroupedAsync(
            p => new { p.Category.Name, p.Price.Amount },
            x => x.Name,
            x => x.Name,
            g => new { Category = g.Key, AveragePrice = g.Average(x => x.Amount) }
        );

        return result.ToDictionary(x => x.Category, x => x.AveragePrice);
    }
}
