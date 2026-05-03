using StreamingApp.Domain.Enums;

namespace StreamingApp.Domain.Entities;

/// <summary>
/// Stores provider transaction details for a user's subscription payment.
/// Uses int PK/FKs for efficient relational joins.
/// </summary>
public class PaymentTransaction
{
    public int Id { get; set; }

    public int UserId { get; set; }

    public int UserSubscriptionId { get; set; }

    public decimal Amount { get; set; }

    public string Currency { get; set; } = "USD";

    public PaymentStatus Status { get; set; } = PaymentStatus.Pending;

    public string? PaymentMethod { get; set; }

    public string? TransactionReference { get; set; }

    public DateTime? PaidAt { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    // Navigation Properties
    public User User { get; set; } = default!;

    public UserSubscription UserSubscription { get; set; } = default!;
}