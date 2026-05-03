using StreamingApp.Domain.Common;

namespace StreamingApp.Domain.Entities;

/// <summary>
/// Payment/transaction record linked to a subscription.
/// PK: int identity (clustered) — internal FK joins.
/// PublicId: Guid v7 — exposed on invoices and email receipts to prevent enumeration.
/// Amount stored as decimal(18,2).
/// </summary>
public class Payment
{
    public int Id { get; set; }
    public Guid PublicId { get; set; } = SequentialGuid.NewId();

    public int UserId { get; set; }           // int FK → User.Id
    public int SubscriptionId { get; set; }   // int FK → Subscription.Id

    public decimal Amount { get; set; }
    public string Currency { get; set; } = "USD";
    public string Status { get; set; } = default!;   // e.g. "Succeeded", "Failed", "Refunded"
    public string? ExternalTransactionId { get; set; }
    public DateTime CreatedAt { get; set; }

    // Navigation
    public User User { get; set; } = default!;
    public Subscription Subscription { get; set; } = default!;
}