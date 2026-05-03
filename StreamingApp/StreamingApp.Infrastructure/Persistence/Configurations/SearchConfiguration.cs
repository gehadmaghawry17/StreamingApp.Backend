using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using StreamingApp.Domain.Entities;

namespace StreamingApp.Infrastructure.Persistence.Configurations;

public class SearchLogConfiguration : IEntityTypeConfiguration<SearchLog>
{
    public void Configure(EntityTypeBuilder<SearchLog> builder)
    {
        builder.HasKey(sl => sl.Id);
        builder.Property(sl => sl.Id)
            .UseIdentityColumn()
            .HasColumnType("bigint");

        builder.Property(sl => sl.Term).IsRequired().HasMaxLength(200);
        builder.Property(sl => sl.CreatedAt).IsRequired();

        builder.HasOne(sl => sl.User)
            .WithMany(u => u.SearchLogs)
            .HasForeignKey(sl => sl.UserId)
            .IsRequired(false)
            .OnDelete(DeleteBehavior.SetNull);

        // No index on Term — aggregate into SearchTrend instead.
        // Deferred: UserId + CreatedAt index for "recent searches" feature
        // builder.HasIndex(sl => new { sl.UserId, sl.CreatedAt })
        //     .HasDatabaseName("IX_SearchLog_UserId_CreatedAt");
    }
}

public class SearchTrendConfiguration : IEntityTypeConfiguration<SearchTrend>
{
    public void Configure(EntityTypeBuilder<SearchTrend> builder)
    {
        // Composite PK: one aggregated row per (term, date)
        builder.HasKey(st => new { st.Term, st.Date });

        builder.Property(st => st.Term).HasMaxLength(200).IsRequired();
        builder.Property(st => st.SearchCount).IsRequired().HasDefaultValue(0L);
        builder.Property(st => st.UpdatedAt).IsRequired();

        // Deferred — add when the table grows past ~50k rows
        // builder.HasIndex(st => new { st.Date, st.SearchCount })
        //     .HasDatabaseName("IX_SearchTrend_Date_SearchCount");
    }
}
