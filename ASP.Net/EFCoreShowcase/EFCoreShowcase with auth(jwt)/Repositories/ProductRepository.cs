using EFCoreShowcase.Data;
using EFCoreShowcase.Interfaces;
using EFCoreShowcase.Models;
using EFCoreShowcase.Models.Auth;
using EFCoreShowcase.Models.DTOs;
using Microsoft.EntityFrameworkCore;

namespace EFCoreShowcase.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly AppDbContext _context;

        public ProductRepository(AppDbContext context)
        {
            _context = context;
        }

        private int ParseUserId(string userId)
        {
            if (!int.TryParse(userId, out int parsedId))
                throw new ArgumentException("Invalid user ID format");
            return parsedId;
        }

        public async Task<IEnumerable<ProductDto>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            return await _context.Products
                .AsNoTracking()
                .Select(p => new ProductDto(p.Id, p.Name, p.Price, p.CategoryId, p.UserId.ToString()))
                .ToListAsync(cancellationToken);
        }

        public async Task<ProductDto?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            return await _context.Products
                .AsNoTracking()
                .Where(p => p.Id == id)
                .Select(p => new ProductDto(p.Id, p.Name, p.Price, p.CategoryId, p.UserId.ToString()))
                .FirstOrDefaultAsync(cancellationToken);
        }

        public async Task<IEnumerable<ProductDto>> GetUserProductsAsync(string userId, CancellationToken cancellationToken = default)
        {
            var parsedUserId = ParseUserId(userId);
            return await _context.Products
                .AsNoTracking()
                .Where(p => p.UserId == parsedUserId)
                .Select(p => new ProductDto(p.Id, p.Name, p.Price, p.CategoryId, p.UserId.ToString()))
                .ToListAsync(cancellationToken);
        }

        public async Task<ProductDto> CreateAsync(CreateProductDto productDto, string userId, CancellationToken cancellationToken = default)
        {
            var parsedUserId = int.Parse(userId);
            
            // Verify user exists
            var user = await _context.Users.FindAsync(new object[] { parsedUserId }, cancellationToken);
            if (user == null)
                throw new ArgumentException("Invalid user ID");

            var product = new Product
            {
                Name = productDto.Name,
                Price = productDto.Price,
                CategoryId = productDto.CategoryId,
                UserId = parsedUserId
            };

            _context.Products.Add(product);
            await _context.SaveChangesAsync(cancellationToken);

            return new ProductDto(product.Id, product.Name, product.Price, product.CategoryId, userId);
        }

        public async Task<ProductDto?> UpdateAsync(int id, UpdateProductDto productDto, CancellationToken cancellationToken = default)
        {
            var product = await _context.Products.FindAsync(new object[] { id }, cancellationToken);
            if (product == null) return null;

            product.Name = productDto.Name;
            product.Price = productDto.Price;
            product.CategoryId = productDto.CategoryId;
            await _context.SaveChangesAsync(cancellationToken);

            return new ProductDto(product.Id, product.Name, product.Price, product.CategoryId, product.UserId.ToString());
        }

        public async Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default)
        {
            var product = await _context.Products.FindAsync(new object[] { id }, cancellationToken);
            if (product == null) return false;

            _context.Products.Remove(product);
            await _context.SaveChangesAsync(cancellationToken);
            return true;
        }

        public async Task<bool> IsProductOwner(int productId, string userId)
        {
            var parsedUserId = ParseUserId(userId);
            return await _context.Products
                .AnyAsync(p => p.Id == productId && p.UserId == parsedUserId);
        }
    }
}
