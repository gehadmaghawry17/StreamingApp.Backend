using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using StreamingApp.Domain.Entities;

namespace StreamingApp.Infrastructure.Persistence.Configurations;

public class ViewingProgressConfiguration : IEntityTypeConfiguration<ViewingProgress>
{
    public void Configure(EntityTypeBuilder<ViewingProgress> builder)
    {
        builder.HasKey(vp => new { vp.UserId, vp.ContentType, vp.ContentId });

        builder.Property(vp => vp.ContentType)
            .HasConversion<string>()
            .HasMaxLength(16)
            .IsRequired();

        builder.Property(vp => vp.ContentId).IsRequired();
        builder.Property(vp => vp.ProgressSeconds).IsRequired();
        builder.Property(vp => vp.DurationSeconds).IsRequired();
        builder.Property(vp => vp.IsCompleted).IsRequired().HasDefaultValue(false);
        builder.Property(vp => vp.LastWatchedAt).IsRequired();
        builder.Property(vp => vp.CreatedAt).IsRequired();

        builder.HasOne(vp => vp.User)
            .WithMany(u => u.ViewingProgresses)
            .HasForeignKey(vp => vp.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        // Continue Watching query:
        //   WHERE UserId = @id AND IsCompleted = 0
        //         AND (ProgressSeconds * 100.0 / DurationSeconds) > 5
        //         AND (ProgressSeconds * 100.0 / DurationSeconds) < 90
        //   ORDER BY LastWatchedAt DESC
        //
        // UserId narrows to the user's rows; LastWatchedAt covers the ORDER BY
        // so SQL Server skips the sort operator on the index seek.
        builder.HasIndex(vp => new { vp.UserId, vp.LastWatchedAt })
            .HasDatabaseName("IX_ViewingProgress_UserId_LastWatchedAt");
    }
}
