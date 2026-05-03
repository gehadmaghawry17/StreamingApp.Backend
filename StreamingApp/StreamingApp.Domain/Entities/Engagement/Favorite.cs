using StreamingApp.Domain.Enums;

namespace StreamingApp.Domain.Entities;

/// <summary>
/// Composite PK: (UserId int, ContentType enum, ContentId int).
/// The composite IS the natural unique key — no surrogate needed.
/// ContentType discriminates between Movie and Series.
/// ContentId references Movie.Id or Series.Id depending on ContentType.
///
/// Note: No navigation properties to Movie/Series because the polymorphic
/// ContentId cannot be expressed as a typed EF Core FK. Resolve in the
/// application layer using ContentType + ContentId.
/// </summary>
public class Favorite
{
    public int UserId { get; set; }
    public ContentType ContentType { get; set; }
    public int ContentId { get; set; }
    public DateTime CreatedAt { get; set; }

    // Navigation
    public User User { get; set; } = default!;
}
