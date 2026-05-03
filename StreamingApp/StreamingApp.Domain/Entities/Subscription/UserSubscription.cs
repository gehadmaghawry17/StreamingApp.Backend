using StreamingApp.Domain.Enums;

namespace StreamingApp.Domain.Entities;

/// <summary>
/// Represents a user's subscription to a specific subscription plan.
/// Uses int PK/FKs for efficient relational joins.
/// </summary>
public class UserSubscription
{
    public int Id { get; set; }

    public int UserId { get; set; }

    public int SubscriptionPlanId { get; set; }

    public SubscriptionStatus Status { get; set; } = SubscriptionStatus.PendingPayment;

    public DateTime StartDate { get; set; }

    public DateTime EndDate { get; set; }

    public bool AutoRenew { get; set; } = true;

    public DateTime? CancelledAt { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public DateTime? UpdatedAt { get; set; }

    // Navigation Properties
    public User User { get; set; } = default!;

    public SubscriptionPlan SubscriptionPlan { get; set; } = default!;

    public ICollection<PaymentTransaction> PaymentTransactions { get; set; } = new List<PaymentTransaction>();
}