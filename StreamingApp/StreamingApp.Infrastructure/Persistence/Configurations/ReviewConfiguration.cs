using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using StreamingApp.Domain.Entities;

namespace StreamingApp.Infrastructure.Persistence.Configurations;

public class ReviewConfiguration : IEntityTypeConfiguration<Review>
{
    public void Configure(EntityTypeBuilder<Review> builder)
    {
        builder.HasKey(r => r.Id);
        builder.Property(r => r.Id).UseIdentityColumn();

        builder.Property(r => r.ContentType)
            .HasConversion<string>()
            .HasMaxLength(16)
            .IsRequired();

        builder.Property(r => r.ContentId).IsRequired();
        builder.Property(r => r.Body).IsRequired().HasMaxLength(4000);
        builder.Property(r => r.IsApproved).IsRequired().HasDefaultValue(false);
        builder.Property(r => r.CreatedAt).IsRequired();
        builder.Property(r => r.UpdatedAt).IsRequired();

        builder.HasOne(r => r.User)
            .WithMany(u => u.Reviews)
            .HasForeignKey(r => r.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        // Deferred — add when review listing UI ships
        // builder.HasIndex(r => new { r.ContentType, r.ContentId, r.CreatedAt })
        //     .HasDatabaseName("IX_Reviews_ContentType_ContentId_CreatedAt");
    }
}
