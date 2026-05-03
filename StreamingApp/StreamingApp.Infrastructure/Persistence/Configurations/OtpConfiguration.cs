using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using StreamingApp.Domain.Entities.Userss;

namespace StreamingApp.Infrastructure.Persistence.Configurations;

public class OtpConfiguration : IEntityTypeConfiguration<Otp>
{
    public void Configure(EntityTypeBuilder<Otp> builder)
    {
        builder.HasKey(o => o.Id);
        builder.Property(o => o.Id)
            .UseIdentityColumn()
            .HasColumnType("bigint");

        builder.Property(o => o.CodeHash).IsRequired().HasMaxLength(512);
        builder.Property(o => o.Type)
            .HasConversion<string>()
            .HasMaxLength(32)
            .IsRequired();
        builder.Property(o => o.ExpiresAt).IsRequired();
        builder.Property(o => o.IsUsed).IsRequired().HasDefaultValue(false);
        builder.Property(o => o.CreatedAt).IsRequired();

        builder.HasOne(o => o.User)
            .WithMany(u => u.Otps)
            .HasForeignKey(o => o.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        // No extra indexes: rows are purged on use/expiry by a background job,
        // keeping the table tiny. CodeHash lookups are fast on a small table.
    }
}
