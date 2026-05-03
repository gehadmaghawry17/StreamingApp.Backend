using StreamingApp.Domain.Enums;

namespace StreamingApp.Domain.Entities;

/// <summary>
/// MVP plans: Monthly and Yearly. No free trial.
/// PK: int identity — tiny seeded table.
/// DurationDays drives EndDate calculation when a Subscription is created.
/// </summary>
public class SubscriptionPlan
{
    public int Id { get; set; }

    public string Name { get; set; } = default!;
    public PlanDuration Duration { get; set; }
    public decimal Price { get; set; }

    /// <summary>e.g. 30 for Monthly, 365 for Yearly.</summary>
    public int DurationDays { get; set; }

    public DateTime CreatedAt { get; set; }

    // Navigation
    public ICollection<Subscription> Subscriptions { get; set; } = [];
}
