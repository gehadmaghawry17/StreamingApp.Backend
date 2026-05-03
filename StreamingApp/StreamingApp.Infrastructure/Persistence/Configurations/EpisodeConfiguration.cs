using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using StreamingApp.Domain.Entities;

namespace StreamingApp.Infrastructure.Persistence.Configurations;

public class EpisodeConfiguration : IEntityTypeConfiguration<Episode>
{
    public void Configure(EntityTypeBuilder<Episode> builder)
    {
        builder.HasKey(e => e.Id);
        builder.Property(e => e.Id).UseIdentityColumn();

        builder.Property(e => e.EpisodeNumber).IsRequired();
        builder.Property(e => e.Title).IsRequired().HasMaxLength(256);
        builder.Property(e => e.Description).HasMaxLength(2000);
        builder.Property(e => e.VideoUrl).IsRequired().HasMaxLength(1024);
        builder.Property(e => e.DurationSeconds).IsRequired();
        builder.Property(e => e.CreatedAt).IsRequired();

        builder.HasOne(e => e.Season)
            .WithMany(s => s.Episodes)
            .HasForeignKey(e => e.SeasonId)
            .OnDelete(DeleteBehavior.Cascade);

        // Episode numbers must be unique within a season
        builder.HasIndex(e => new { e.SeasonId, e.EpisodeNumber })
            .IsUnique()
            .HasDatabaseName("UX_Episodes_SeasonId_EpisodeNumber");
    }
}
