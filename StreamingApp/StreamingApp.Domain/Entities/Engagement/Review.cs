using StreamingApp.Domain.Enums;

namespace StreamingApp.Domain.Entities;

/// <summary>
/// User-submitted text review for a Movie or Series.
/// PK: int identity.
/// ContentType: Movie or Series (reviews are not per-episode).
/// </summary>
public class Review
{
    public int Id { get; set; }

    public int UserId { get; set; }
    public ContentType ContentType { get; set; }
    public int ContentId { get; set; }

    public string Body { get; set; } = default!;
    public bool IsApproved { get; set; }    // content moderation flag — false by default
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }

    // Navigation
    public User User { get; set; } = default!;
}
