using StreamingApp.Domain.Enums;

namespace StreamingApp.Domain.Entities;

/// <summary>
/// One rating per user per content item — enforced at PK level.
/// Composite PK: (UserId int, ContentType enum, ContentId int).
/// ContentType: Movie or Series only (ratings are not per-episode in the MVP).
/// Score stored with precision (4,1): supports values like 7.5 up to 10.0.
/// </summary>
public class Rating
{
    public int UserId { get; set; }
    public ContentType ContentType { get; set; }
    public int ContentId { get; set; }

    public decimal Score { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }

    // Navigation
    public User User { get; set; } = default!;
}
