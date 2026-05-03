using StreamingApp.Domain.Enums;

namespace StreamingApp.Domain.Entities;

/// <summary>
/// Admin-managed promoted content for carousels (Top Searches, Trending).
/// PK: int identity — tiny table, managed by admin panel.
/// ContentType: Movie or Series.
/// DisplayOrder controls the carousel position.
/// </summary>
public class PromotedContent
{
    public int Id { get; set; }

    public ContentType ContentType { get; set; }
    public int ContentId { get; set; }   // references Movie.Id or Series.Id

    public string Label { get; set; } = default!;   // e.g. "Trending Now", "Staff Pick"
    public int DisplayOrder { get; set; }
    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}
