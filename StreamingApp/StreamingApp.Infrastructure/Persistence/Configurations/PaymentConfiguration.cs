using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using StreamingApp.Domain.Entities;

namespace StreamingApp.Infrastructure.Persistence.Configurations;

public class PaymentConfiguration : IEntityTypeConfiguration<Payment>
{
    public void Configure(EntityTypeBuilder<Payment> builder)
    {
        builder.HasKey(p => p.Id);
        builder.Property(p => p.Id).UseIdentityColumn();

        builder.Property(p => p.PublicId).IsRequired();
        builder.Property(p => p.Amount).HasPrecision(18, 2).IsRequired();
        builder.Property(p => p.Currency).IsRequired().HasMaxLength(8).HasDefaultValue("USD");
        builder.Property(p => p.Status).IsRequired().HasMaxLength(32);
        builder.Property(p => p.ExternalTransactionId).HasMaxLength(256);
        builder.Property(p => p.CreatedAt).IsRequired();

        // Resolves payment receipt in emails/invoices without exposing int PK
        builder.HasIndex(p => p.PublicId)
            .IsUnique()
            .HasDatabaseName("UX_Payments_PublicId");

        builder.HasOne(p => p.User)
            .WithMany(u => u.Payments)
            .HasForeignKey(p => p.UserId)
            .OnDelete(DeleteBehavior.Restrict);   // keep payment records if user is deleted

        builder.HasOne(p => p.Subscription)
            .WithMany(s => s.Payments)
            .HasForeignKey(p => p.SubscriptionId)
            .OnDelete(DeleteBehavior.Restrict);

        // Deferred — add when billing history page ships
        // builder.HasIndex(p => new { p.UserId, p.CreatedAt })
        //     .HasDatabaseName("IX_Payments_UserId_CreatedAt");
    }
}
