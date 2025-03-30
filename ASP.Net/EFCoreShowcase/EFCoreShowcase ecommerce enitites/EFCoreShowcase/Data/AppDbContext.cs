using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using EFCoreShowcase.Entities.Interfaces;
using EFCoreShowcase.Entities;
using EFCoreShowcase.Common.Exceptions;

namespace EFCoreShowcase.Data;

public class AppDbContext : DbContext
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly ILogger<AppDbContext> _logger;

    public AppDbContext(
        DbContextOptions<AppDbContext> options,
        IHttpContextAccessor httpContextAccessor,
        ILogger<AppDbContext> logger) : base(options)
    {
        _httpContextAccessor = httpContextAccessor;
        _logger = logger;
    }

    public DbSet<Product> Products { get; set; } = null!;
    public DbSet<Category> Categories { get; set; } = null!;
    public DbSet<Customer> Customers { get; set; } = null!;
    public DbSet<Order> Orders { get; set; } = null!;
    public DbSet<OrderItem> OrderItems { get; set; } = null!;
    public DbSet<Address> Addresses { get; set; } = null!;
    public DbSet<ProductVideo> ProductVideos { get; set; } = null!;
    public DbSet<ProductImage> ProductImages { get; set; } = null!;
    public DbSet<ProductSpecification> ProductSpecifications { get; set; } = null!;
    public DbSet<ProductReview> ProductReviews { get; set; } = null!;
    public DbSet<ReviewImage> ReviewImages { get; set; } = null!;
    public DbSet<ReviewHelpfulness> ReviewHelpfulness { get; set; } = null!;
    public DbSet<ProductQuestion> ProductQuestions { get; set; } = null!;
    public DbSet<ProductAnswer> ProductAnswers { get; set; } = null!;

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            var entries = ChangeTracker.Entries<AuditableEntity>()
                .Where(e => e.State != EntityState.Unchanged);

            foreach (var entry in entries)
            {
                entry.Entity.OnSaving();
            }

            var result = await base.SaveChangesAsync(cancellationToken);

            foreach (var entry in entries)
            {
                entry.Entity.OnSaved();
            }

            return result;
        }
        catch (DbUpdateConcurrencyException ex)
        {
            _logger.LogError(ex, "Concurrency conflict detected");
            throw new DomainException("The record was modified by another user");
        }
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Apply all configurations from assembly
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);

        // Global query filters for soft delete
        modelBuilder.Entity<AuditableEntity>()
            .HasQueryFilter(e => !e.IsDeleted);
    }
}
