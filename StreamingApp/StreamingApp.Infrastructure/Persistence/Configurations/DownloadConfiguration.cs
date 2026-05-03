using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using StreamingApp.Domain.Entities;

namespace StreamingApp.Infrastructure.Persistence.Configurations;

public class DownloadConfiguration : IEntityTypeConfiguration<Download>
{
    public void Configure(EntityTypeBuilder<Download> builder)
    {
        builder.HasKey(d => d.Id);
        // Generated in domain via SequentialGuid.NewId() — not by the DB
        builder.Property(d => d.Id).ValueGeneratedNever();

        builder.Property(d => d.ExpiresAt).IsRequired();
        builder.Property(d => d.IsRevoked).IsRequired().HasDefaultValue(false);
        builder.Property(d => d.CreatedAt).IsRequired();

        // XOR: exactly one of MovieId / EpisodeId must be non-null
        builder.ToTable(t => t.HasCheckConstraint(
            "CK_Downloads_ContentTarget",
            "([MovieId] IS NOT NULL AND [EpisodeId] IS NULL) OR ([MovieId] IS NULL AND [EpisodeId] IS NOT NULL)"));

        builder.HasOne(d => d.User)
            .WithMany(u => u.Downloads)
            .HasForeignKey(d => d.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(d => d.Movie)
            .WithMany()
            .HasForeignKey(d => d.MovieId)
            .IsRequired(false)
            .OnDelete(DeleteBehavior.SetNull);

        builder.HasOne(d => d.Episode)
            .WithMany()
            .HasForeignKey(d => d.EpisodeId)
            .IsRequired(false)
            .OnDelete(DeleteBehavior.SetNull);
    }
}
