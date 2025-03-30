using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EFCoreShowcase.Data.Configurations;

public class ProductConfiguration : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        builder.Property(p => p.Price)
            .HasPrecision(18, 2);

        builder.Property(p => p.SKU)
            .HasMaxLength(50)
            .IsUnicode(false);

        builder.HasIndex(p => p.SKU)
            .IsUnique();

        builder.HasQueryFilter(p => !p.IsDeleted);

        builder.HasOne(p => p.Category)
            .WithMany(c => c.Products)
            .HasForeignKey(p => p.CategoryId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Property(p => p.IsLowStock)
            .HasComputedColumnSql("CASE WHEN [StockQuantity] <= [MinStockThreshold] THEN 1 ELSE 0 END");

        builder.OwnsOne(p => p.Price, priceBuilder =>
        {
            priceBuilder.Property(m => m.Amount)
                .HasColumnName("Price")
                .HasPrecision(18, 2);

            priceBuilder.Property(m => m.Currency)
                .HasColumnName("Currency")
                .HasMaxLength(3);
        });

        builder.HasIndex(p => p.Name);
        builder.HasIndex(p => p.Price);
        builder.HasIndex(p => p.IsLowStock);
        builder.HasIndex(p => p.CreatedAt);
    }
}
