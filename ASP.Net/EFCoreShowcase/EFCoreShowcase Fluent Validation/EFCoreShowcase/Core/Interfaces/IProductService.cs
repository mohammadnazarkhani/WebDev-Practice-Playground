using EFCoreShowcase.DTOs;
using EFCoreShowcase.Models;
using EFCoreShowcase.Models.Requests;
using EFCoreShowcase.Models.Responses;

namespace EFCoreShowcase.Core.Interfaces;

public interface IProductService
{
    Task<PagedResponse<Product>> SearchProducts(ProductSearchParameters parameters);
    Task<Product> CreateProduct(ProductDto productDto);
    Task<Product?> GetProduct(int id);
    Task<Product> UpdateProduct(int id, ProductDto productDto);
    Task DeleteProduct(int id);
}
