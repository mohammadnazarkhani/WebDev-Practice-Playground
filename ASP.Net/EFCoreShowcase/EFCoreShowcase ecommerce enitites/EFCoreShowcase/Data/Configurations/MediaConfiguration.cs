using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using EFCoreShowcase.Entities;

namespace EFCoreShowcase.Data.Configurations;

public class ProductImageConfiguration : IEntityTypeConfiguration<ProductImage>
{
    public void Configure(EntityTypeBuilder<ProductImage> builder)
    {
        builder.Property(x => x.ImageUrl)
            .IsRequired()
            .HasMaxLength(2000);

        builder.HasIndex(x => x.ProductId);
    }
}

public class ProductVideoConfiguration : IEntityTypeConfiguration<ProductVideo>
{
    public void Configure(EntityTypeBuilder<ProductVideo> builder)
    {
        builder.Property(x => x.Title)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(x => x.Description)
            .IsRequired()
            .HasMaxLength(1000);

        builder.Property(x => x.VideoUrl)
            .IsRequired()
            .HasMaxLength(2000);

        builder.Property(x => x.ThumbnailUrl)
            .IsRequired()
            .HasMaxLength(2000);

        builder.HasIndex(x => x.ProductId);
    }
}

public class ReviewImageConfiguration : IEntityTypeConfiguration<ReviewImage>
{
    public void Configure(EntityTypeBuilder<ReviewImage> builder)
    {
        builder.Property(x => x.ImageUrl)
            .IsRequired()
            .HasMaxLength(2000);

        builder.HasIndex(x => x.ReviewId);
    }
}
