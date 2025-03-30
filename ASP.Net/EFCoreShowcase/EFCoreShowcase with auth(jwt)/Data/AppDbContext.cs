using EFCoreShowcase.Models;
using EFCoreShowcase.Models.Auth;
using Microsoft.EntityFrameworkCore;

namespace EFCoreShowcase.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<Product> Products { get; set; }
    public DbSet<Category> Categories { get; set; }
    public DbSet<User> Users { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.Entity<Product>(entity =>
        {
            entity.Property(p => p.Price)
                .HasPrecision(18, 2);

            entity.HasOne(p => p.Category)
                .WithMany(c => c.Products)
                .HasForeignKey(p => p.CategoryId)
                .OnDelete(DeleteBehavior.Cascade); // Changed from Restrict to Cascade

            entity.HasOne(p => p.User)
                .WithMany(u => u.Products)
                .HasForeignKey(p => p.UserId)
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired();

            entity.Property(p => p.CreatedAt)
                .HasDefaultValueSql("GETUTCDATE()");
        });

        builder.Entity<Category>(entity =>
        {
            entity.HasIndex(c => c.Name)
                .IsUnique();
        });

        builder.Entity<User>(entity =>
        {
            entity.HasIndex(u => u.Username)
                .IsUnique();

            entity.HasIndex(u => u.Email)
                .IsUnique();
        });
    }
}
