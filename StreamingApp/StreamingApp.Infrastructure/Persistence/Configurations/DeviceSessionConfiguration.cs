using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using StreamingApp.Domain.Entities;

namespace StreamingApp.Infrastructure.Persistence.Configurations;

public class DeviceSessionConfiguration : IEntityTypeConfiguration<DeviceSession>
{
    public void Configure(EntityTypeBuilder<DeviceSession> builder)
    {
        builder.HasKey(ds => ds.Id);
        // Guid v7 generated in domain layer via SequentialGuid.NewId()
        builder.Property(ds => ds.Id).ValueGeneratedNever();

        builder.Property(ds => ds.DeviceInfo).HasMaxLength(512);
        builder.Property(ds => ds.IpAddress).HasMaxLength(45);   // max length for IPv6
        builder.Property(ds => ds.CreatedAt).IsRequired();
        builder.Property(ds => ds.LastActiveAt).IsRequired();

        builder.HasOne(ds => ds.User)
            .WithMany(u => u.DeviceSessions)
            .HasForeignKey(ds => ds.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        // MVP constraint: one active session per user
        builder.HasIndex(ds => ds.UserId)
            .IsUnique()
            .HasDatabaseName("UX_DeviceSessions_UserId");
    }
}
