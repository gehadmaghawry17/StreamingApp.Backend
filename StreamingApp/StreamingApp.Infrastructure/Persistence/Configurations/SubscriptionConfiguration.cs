using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using StreamingApp.Domain.Entities;
using StreamingApp.Domain.Enums;

namespace StreamingApp.Infrastructure.Persistence.Configurations;

public class SubscriptionConfiguration : IEntityTypeConfiguration<Subscription>
{
    public void Configure(EntityTypeBuilder<Subscription> builder)
    {
        builder.HasKey(s => s.Id);
        builder.Property(s => s.Id).UseIdentityColumn();

        builder.Property(s => s.StartDate).IsRequired();
        builder.Property(s => s.EndDate).IsRequired();
        builder.Property(s => s.IsActive).IsRequired().HasDefaultValue(false);
        builder.Property(s => s.CreatedAt).IsRequired();

        builder.HasOne(s => s.User)
            .WithMany(u => u.Subscriptions)
            .HasForeignKey(s => s.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(s => s.Plan)
            .WithMany(p => p.Subscriptions)
            .HasForeignKey(s => s.PlanId)
            .OnDelete(DeleteBehavior.Restrict);   // do not delete a plan that has subscribers

        // Active subscription check — runs on every gated content request:
        //   WHERE UserId = @id AND IsActive = 1 AND EndDate > GETUTCDATE()
        // Column order: UserId (equality) → IsActive (equality) → EndDate (range)
        builder.HasIndex(s => new { s.UserId, s.IsActive, s.EndDate })
            .HasDatabaseName("IX_Subscriptions_UserId_IsActive_EndDate");
    }
}

