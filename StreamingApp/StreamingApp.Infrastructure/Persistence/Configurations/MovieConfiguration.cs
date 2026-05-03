using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using StreamingApp.Domain.Entities;

namespace StreamingApp.Infrastructure.Persistence.Configurations;

public class MovieConfiguration : IEntityTypeConfiguration<Movie>
{
    public void Configure(EntityTypeBuilder<Movie> builder)
    {
        builder.HasKey(m => m.Id);
        builder.Property(m => m.Id).UseIdentityColumn();

        builder.Property(m => m.PublicId).IsRequired();
        builder.Property(m => m.Title).IsRequired().HasMaxLength(256);
        builder.Property(m => m.Slug).IsRequired().HasMaxLength(300);
        builder.Property(m => m.Description).HasMaxLength(2000);
        builder.Property(m => m.ThumbnailUrl).HasMaxLength(1024);
        builder.Property(m => m.TrailerUrl).IsRequired().HasMaxLength(1024);
        builder.Property(m => m.VideoUrl).IsRequired().HasMaxLength(1024);
        builder.Property(m => m.DurationSeconds).IsRequired();
        builder.Property(m => m.ReleaseYear).IsRequired();
        builder.Property(m => m.AverageRating).HasPrecision(3, 2).HasDefaultValue(0m);
        builder.Property(m => m.IsPublished).IsRequired().HasDefaultValue(false);
        builder.Property(m => m.CreatedAt).IsRequired();
        builder.Property(m => m.UpdatedAt).IsRequired();

        // GET /movies/{publicId}
        builder.HasIndex(m => m.PublicId)
            .IsUnique()
            .HasDatabaseName("UX_Movies_PublicId");

        // GET /movies/{slug}
        builder.HasIndex(m => m.Slug)
            .IsUnique()
            .HasDatabaseName("UX_Movies_Slug");

        // Homepage "latest published" feed: WHERE IsPublished = 1 ORDER BY CreatedAt DESC
        builder.HasIndex(m => new { m.IsPublished, m.CreatedAt })
            .HasDatabaseName("IX_Movies_IsPublished_CreatedAt");
    }
}
