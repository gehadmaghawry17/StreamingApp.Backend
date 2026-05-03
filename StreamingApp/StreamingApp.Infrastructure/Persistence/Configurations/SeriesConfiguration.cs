using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using StreamingApp.Domain.Entities;

namespace StreamingApp.Infrastructure.Persistence.Configurations;

public class SeriesConfiguration : IEntityTypeConfiguration<Series>
{
    public void Configure(EntityTypeBuilder<Series> builder)
    {
        builder.HasKey(s => s.Id);
        builder.Property(s => s.Id).UseIdentityColumn();

        builder.Property(s => s.PublicId).IsRequired();
        builder.Property(s => s.Title).IsRequired().HasMaxLength(256);
        builder.Property(s => s.Slug).IsRequired().HasMaxLength(300);
        builder.Property(s => s.Description).HasMaxLength(2000);
        builder.Property(s => s.ThumbnailUrl).HasMaxLength(1024);
        builder.Property(s => s.TrailerUrl).IsRequired().HasMaxLength(1024);
        builder.Property(s => s.AverageRating).HasPrecision(3, 2).HasDefaultValue(0m);
        builder.Property(s => s.IsPublished).IsRequired().HasDefaultValue(false);
        builder.Property(s => s.CreatedAt).IsRequired();
        builder.Property(s => s.UpdatedAt).IsRequired();

        builder.HasIndex(s => s.PublicId)
            .IsUnique()
            .HasDatabaseName("UX_Series_PublicId");

        builder.HasIndex(s => s.Slug)
            .IsUnique()
            .HasDatabaseName("UX_Series_Slug");

        builder.HasIndex(s => new { s.IsPublished, s.CreatedAt })
            .HasDatabaseName("IX_Series_IsPublished_CreatedAt");
    }
}
