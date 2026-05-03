using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using StreamingApp.Domain.Entities;

namespace StreamingApp.Infrastructure.Persistence.Configurations;

public class PromotedContentConfiguration : IEntityTypeConfiguration<PromotedContent>
{
    public void Configure(EntityTypeBuilder<PromotedContent> builder)
    {
        builder.HasKey(pc => pc.Id);
        builder.Property(pc => pc.Id).UseIdentityColumn();

        builder.Property(pc => pc.ContentType)
            .HasConversion<string>()
            .HasMaxLength(16)
            .IsRequired();

        builder.Property(pc => pc.ContentId).IsRequired();
        builder.Property(pc => pc.Label).IsRequired().HasMaxLength(128);
        builder.Property(pc => pc.DisplayOrder).IsRequired();
        builder.Property(pc => pc.IsActive).IsRequired().HasDefaultValue(false);
        builder.Property(pc => pc.CreatedAt).IsRequired();
        builder.Property(pc => pc.UpdatedAt).IsRequired();

        // Trending / Top Searches carousel:
        //   WHERE IsActive = 1 ORDER BY DisplayOrder ASC
        builder.HasIndex(pc => new { pc.IsActive, pc.DisplayOrder })
            .HasDatabaseName("IX_PromotedContent_IsActive_DisplayOrder");
    }
}
