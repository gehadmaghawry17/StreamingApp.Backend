using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using StreamingApp.Domain.Entities;

namespace StreamingApp.Infrastructure.Persistence.Configurations;

public class SeasonConfiguration : IEntityTypeConfiguration<Season>
{
    public void Configure(EntityTypeBuilder<Season> builder)
    {
        builder.HasKey(s => s.Id);
        builder.Property(s => s.Id).UseIdentityColumn();

        builder.Property(s => s.SeasonNumber).IsRequired();
        builder.Property(s => s.Title).IsRequired().HasMaxLength(256);
        builder.Property(s => s.CreatedAt).IsRequired();

        builder.HasOne(s => s.Series)
            .WithMany(sr => sr.Seasons)
            .HasForeignKey(s => s.SeriesId)
            .OnDelete(DeleteBehavior.Cascade);

        // Season numbers must be unique within a series
        builder.HasIndex(s => new { s.SeriesId, s.SeasonNumber })
            .IsUnique()
            .HasDatabaseName("UX_Seasons_SeriesId_SeasonNumber");
    }
}
