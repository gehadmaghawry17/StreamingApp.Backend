using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using StreamingApp.Domain.Entities;
using StreamingApp.Domain.Enums;

namespace StreamingApp.Infrastructure.Persistence.Configurations;

public class FavoriteConfiguration : IEntityTypeConfiguration<Favorite>
{
    public void Configure(EntityTypeBuilder<Favorite> builder)
    {
        // Composite PK — enforces "one favourite per user per content item" at DB level
        builder.HasKey(f => new { f.UserId, f.ContentType, f.ContentId });

        // Store enum as string — human-readable in the DB, no lookup table needed
        builder.Property(f => f.ContentType)
            .HasConversion<string>()
            .HasMaxLength(16)
            .IsRequired();

        builder.Property(f => f.ContentId).IsRequired();
        builder.Property(f => f.CreatedAt).IsRequired();

        builder.HasOne(f => f.User)
            .WithMany(u => u.Favorites)
            .HasForeignKey(f => f.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        // Supports: "which users favourited content X of type Y?"
        builder.HasIndex(f => new { f.ContentType, f.ContentId })
            .HasDatabaseName("IX_Favorites_ContentType_ContentId");
    }
}
