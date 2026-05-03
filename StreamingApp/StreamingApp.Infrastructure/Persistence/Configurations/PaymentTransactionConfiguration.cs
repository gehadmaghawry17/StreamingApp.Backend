using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using StreamingApp.Domain.Entities;

namespace StreamingApp.Infrastructure.Persistence.Configurations;

public class PaymentTransactionConfiguration : IEntityTypeConfiguration<PaymentTransaction>
{
    public void Configure(EntityTypeBuilder<PaymentTransaction> builder)
    {
        builder.ToTable("PaymentTransactions");

        builder.HasKey(pt => pt.Id);

        builder.Property(pt => pt.Id)
            .UseIdentityColumn();

        builder.Property(pt => pt.Amount)
            .HasPrecision(18, 2)
            .IsRequired();

        builder.Property(pt => pt.Currency)
            .HasMaxLength(8)
            .HasDefaultValue("USD")
            .IsRequired();

        builder.Property(pt => pt.Status)
            .HasConversion<string>()
            .HasMaxLength(32)
            .IsRequired();

        builder.Property(pt => pt.PaymentMethod)
            .HasMaxLength(64);

        builder.Property(pt => pt.TransactionReference)
            .HasMaxLength(256);

        builder.Property(pt => pt.PaidAt);

        builder.Property(pt => pt.CreatedAt)
            .IsRequired();

        builder.HasOne(pt => pt.UserSubscription)
            .WithMany(us => us.PaymentTransactions)
            .HasForeignKey(pt => pt.UserSubscriptionId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(pt => pt.User)
            .WithMany()
            .HasForeignKey(pt => pt.UserId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}