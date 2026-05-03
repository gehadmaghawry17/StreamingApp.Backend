namespace StreamingApp.Domain.Entities;

/// <summary>
/// Represents one subscription period for a user.
/// PK: int identity — low volume, one active subscription per user.
/// IsActive is maintained by the app layer on renewal or expiry.
/// Playback service checks: IsActive = true AND EndDate > UtcNow.
/// </summary>
public class Subscription
{
    public int Id { get; set; }

    public int UserId { get; set; }     // int FK → User.Id
    public int PlanId { get; set; }     // int FK → SubscriptionPlan.Id

    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; set; }

    // Navigation
    public User User { get; set; } = default!;
    public SubscriptionPlan Plan { get; set; } = default!;
    public ICollection<Payment> Payments { get; set; } = [];
}
