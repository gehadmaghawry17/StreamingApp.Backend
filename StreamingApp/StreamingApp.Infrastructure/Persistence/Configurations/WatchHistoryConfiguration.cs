using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using StreamingApp.Domain.Entities;

namespace StreamingApp.Infrastructure.Persistence.Configurations;

public class WatchHistoryConfiguration : IEntityTypeConfiguration<WatchHistory>
{
    public void Configure(EntityTypeBuilder<WatchHistory> builder)
    {
        builder.HasKey(wh => wh.Id);
        builder.Property(wh => wh.Id)
            .UseIdentityColumn()
            .HasColumnType("bigint");

        builder.Property(wh => wh.UserId).IsRequired();
        builder.Property(wh => wh.WatchedSeconds).IsRequired();
        builder.Property(wh => wh.WatchedAt).IsRequired();

        // XOR: exactly one of MovieId / EpisodeId must be non-null
        builder.ToTable(t => t.HasCheckConstraint(
            "CK_WatchHistory_ContentTarget",
            "([MovieId] IS NOT NULL AND [EpisodeId] IS NULL) OR ([MovieId] IS NULL AND [EpisodeId] IS NOT NULL)"));

        builder.HasOne(wh => wh.User)
            .WithMany(u => u.WatchHistories)
            .HasForeignKey(wh => wh.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(wh => wh.Movie)
            .WithMany(m => m.WatchHistories)
            .HasForeignKey(wh => wh.MovieId)
            .IsRequired(false)
            .OnDelete(DeleteBehavior.SetNull);

        builder.HasOne(wh => wh.Episode)
            .WithMany(e => e.WatchHistories)
            .HasForeignKey(wh => wh.EpisodeId)
            .IsRequired(false)
            .OnDelete(DeleteBehavior.SetNull);

        // Deferred — add when watch history page ships
        // builder.HasIndex(wh => new { wh.UserId, wh.WatchedAt })
        //     .HasDatabaseName("IX_WatchHistory_UserId_WatchedAt");
    }
}
