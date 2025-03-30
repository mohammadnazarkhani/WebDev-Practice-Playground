using EFCoreShowcase.Models.DTOs;

namespace EFCoreShowcase.Interfaces
{
    public interface IProductRepository
    {
        Task<IEnumerable<ProductDto>> GetAllAsync(CancellationToken cancellationToken = default);
        Task<ProductDto?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
        Task<IEnumerable<ProductDto>> GetUserProductsAsync(string userId, CancellationToken cancellationToken = default);
        Task<ProductDto> CreateAsync(CreateProductDto productDto, string userId, CancellationToken cancellationToken = default);
        Task<ProductDto?> UpdateAsync(int id, UpdateProductDto productDto, CancellationToken cancellationToken = default);
        Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default);
        Task<bool> IsProductOwner(int productId, string userId);
    }
}
