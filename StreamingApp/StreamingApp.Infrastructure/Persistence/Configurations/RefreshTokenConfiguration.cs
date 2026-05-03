using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using StreamingApp.Domain.Entities;

namespace StreamingApp.Infrastructure.Persistence.Configurations;

public class RefreshTokenConfiguration : IEntityTypeConfiguration<RefreshToken>
{
    public void Configure(EntityTypeBuilder<RefreshToken> builder)
    {
        builder.HasKey(rt => rt.Id);
        // Guid v7 generated in domain layer via SequentialGuid.NewId()
        builder.Property(rt => rt.Id).ValueGeneratedNever();

        builder.Property(rt => rt.TokenHash).IsRequired().HasMaxLength(512);
        builder.Property(rt => rt.ExpiresAt).IsRequired();
        builder.Property(rt => rt.IsRevoked).IsRequired().HasDefaultValue(false);
        builder.Property(rt => rt.CreatedAt).IsRequired();

        builder.HasOne(rt => rt.User)
            .WithMany(u => u.RefreshTokens)
            .HasForeignKey(rt => rt.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        // Token revocation / renewal lookup — always by hash, never by Id
        builder.HasIndex(rt => rt.TokenHash)
            .IsUnique()
            .HasDatabaseName("UX_RefreshTokens_TokenHash");
    }
}
