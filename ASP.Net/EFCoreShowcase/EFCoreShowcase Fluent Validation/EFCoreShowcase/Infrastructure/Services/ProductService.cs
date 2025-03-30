using EFCoreShowcase.Core.Interfaces;
using EFCoreShowcase.Data;
using EFCoreShowcase.DTOs;
using EFCoreShowcase.Models;
using EFCoreShowcase.Models.Requests;
using EFCoreShowcase.Models.Responses;
using Microsoft.EntityFrameworkCore;

namespace EFCoreShowcase.Infrastructure.Services;

public class ProductService : IProductService
{
    private readonly AppDbContext _context;

    public ProductService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<PagedResponse<Product>> SearchProducts(ProductSearchParameters parameters)
    {
        var query = _context.Products
            .Include(p => p.Category)
            .AsQueryable();

        if (!string.IsNullOrEmpty(parameters.SearchTerm))
        {
            query = query.Where(p => p.Name.Contains(parameters.SearchTerm)
                || p.Description.Contains(parameters.SearchTerm));
        }

        if (parameters.MinPrice.HasValue)
        {
            query = query.Where(p => p.Price >= parameters.MinPrice.Value);
        }

        if (parameters.MaxPrice.HasValue)
        {
            query = query.Where(p => p.Price <= parameters.MaxPrice.Value);
        }

        if (parameters.CategoryId.HasValue)
        {
            query = query.Where(p => p.CategoryId == parameters.CategoryId.Value);
        }

        var totalItems = await query.CountAsync();
        var totalPages = (int)Math.Ceiling(totalItems / (double)parameters.PageSize);

        var items = await query
            .Skip((parameters.Page - 1) * parameters.PageSize)
            .Take(parameters.PageSize)
            .ToListAsync();

        return new PagedResponse<Product>
        {
            Items = items,
            TotalItems = totalItems,
            PageNumber = parameters.Page,
            TotalPages = totalPages,
            HasNextPage = parameters.Page < totalPages,
            HasPreviousPage = parameters.Page > 1
        };
    }

    public async Task<Product> CreateProduct(ProductDto productDto)
    {
        var product = new Product
        {
            Name = productDto.Name,
            Price = productDto.Price,
            Description = productDto.Description,
            CategoryId = productDto.CategoryId
        };

        _context.Products.Add(product);
        await _context.SaveChangesAsync();

        // Reload the product with category data
        return await _context.Products
            .Include(p => p.Category)
            .FirstAsync(p => p.Id == product.Id);
    }

    public async Task<Product?> GetProduct(int id)
        => await _context.Products
            .Include(p => p.Category)
            .FirstOrDefaultAsync(p => p.Id == id);

    public async Task<Product> UpdateProduct(int id, ProductDto productDto)
    {
        var product = await _context.Products.FindAsync(id);
        if (product == null)
            throw new KeyNotFoundException($"Product with ID {id} not found.");

        product.Name = productDto.Name;
        product.Price = productDto.Price;
        product.Description = productDto.Description;
        product.CategoryId = productDto.CategoryId;

        await _context.SaveChangesAsync();
        return product;
    }

    public async Task DeleteProduct(int id)
    {
        var product = await _context.Products.FindAsync(id);
        if (product == null)
            throw new KeyNotFoundException($"Product with ID {id} not found.");

        _context.Products.Remove(product);
        await _context.SaveChangesAsync();
    }
}
