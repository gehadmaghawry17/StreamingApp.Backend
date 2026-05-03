using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using StreamingApp.Domain.Entities;

namespace StreamingApp.Infrastructure.Persistence.Configurations;

public class SeriesGenreConfiguration : IEntityTypeConfiguration<SeriesGenre>
{
    public void Configure(EntityTypeBuilder<SeriesGenre> builder)
    {
        builder.HasKey(sg => new { sg.SeriesId, sg.GenreId });

        builder.HasOne(sg => sg.Series)
            .WithMany(s => s.SeriesGenres)
            .HasForeignKey(sg => sg.SeriesId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(sg => sg.Genre)
            .WithMany(g => g.SeriesGenres)
            .HasForeignKey(sg => sg.GenreId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(sg => new { sg.GenreId, sg.SeriesId })
            .HasDatabaseName("IX_SeriesGenre_GenreId_SeriesId");
    }
}
