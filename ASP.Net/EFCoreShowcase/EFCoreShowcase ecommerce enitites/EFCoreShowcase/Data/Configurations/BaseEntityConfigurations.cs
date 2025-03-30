using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using EFCoreShowcase.Entities;

namespace EFCoreShowcase.Data.Configurations;

public class AuditableEntityConfiguration : IEntityTypeConfiguration<AuditableEntity>
{
    public void Configure(EntityTypeBuilder<AuditableEntity> builder)
    {
        builder.UseTpcMappingStrategy();

        builder.Property(e => e.CreatedAt)
            .HasDefaultValueSql("GETUTCDATE()");

        builder.Property(e => e.IsDeleted)
            .HasDefaultValue(false);
    }
}

public class MediaEntityBaseConfiguration : IEntityTypeConfiguration<MediaEntityBase>
{
    public void Configure(EntityTypeBuilder<MediaEntityBase> builder)
    {
        builder.UseTpcMappingStrategy();

        builder.HasQueryFilter(e => !e.IsDeleted);

        builder.Property(e => e.CreatedAt)
            .HasDefaultValueSql("GETUTCDATE()")
            .IsRequired();

        builder.Property(e => e.UpdatedAt)
            .IsRequired(false);

        builder.Property(e => e.DeletedAt)
            .IsRequired(false);

        builder.Property(e => e.CreatedBy)
            .HasMaxLength(100)
            .IsRequired(false);

        builder.Property(e => e.UpdatedBy)
            .HasMaxLength(100)
            .IsRequired(false);

        builder.Property(e => e.DeletedBy)
            .HasMaxLength(100)
            .IsRequired(false);

        builder.HasIndex(e => e.CreatedAt);
        builder.HasIndex(e => e.IsDeleted);
    }
}
