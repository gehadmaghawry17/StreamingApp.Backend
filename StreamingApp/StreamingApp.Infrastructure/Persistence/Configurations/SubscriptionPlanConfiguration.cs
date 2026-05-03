using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using StreamingApp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StreamingApp.Infrastructure.Persistence.Configurations;

public class SubscriptionPlanConfiguration : IEntityTypeConfiguration<SubscriptionPlan>
{
    public void Configure(EntityTypeBuilder<SubscriptionPlan> builder)
    {
        builder.HasKey(sp => sp.Id);
        builder.Property(sp => sp.Id).UseIdentityColumn();

        builder.Property(sp => sp.Name).IsRequired().HasMaxLength(64);
        builder.Property(sp => sp.Duration)
            .HasConversion<string>()
            .HasMaxLength(16)
            .IsRequired();
        builder.Property(sp => sp.Price).HasPrecision(18, 2).IsRequired();
        builder.Property(sp => sp.DurationDays).IsRequired();
        builder.Property(sp => sp.CreatedAt).IsRequired();
    }
}
