using StreamingApp.Domain.Common;

namespace StreamingApp.Domain.Entities;

/// <summary>
/// PK: int identity (clustered).
/// PublicId: Guid v7 — exposed in API routes and CDN paths.
/// </summary>
public class Series
{
    public int Id { get; set; }
    public Guid PublicId { get; set; } = SequentialGuid.NewId();

    public string Title { get; set; } = default!;
    public string Slug { get; set; } = default!;
    public string? Description { get; set; }
    public string? ThumbnailUrl { get; set; }
    public string TrailerUrl { get; set; } = default!;
    public decimal AverageRating { get; set; }
    public bool IsPublished { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }

    // Navigation
    public ICollection<Season> Seasons { get; set; } = [];
    public ICollection<SeriesGenre> SeriesGenres { get; set; } = [];
}
