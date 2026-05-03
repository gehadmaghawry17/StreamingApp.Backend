using StreamingApp.Domain.Common;

namespace StreamingApp.Domain.Entities;

/// <summary>
/// PK: int identity (clustered) — used for all FK joins.
/// PublicId: Guid v7 — used in API routes (/movies/{publicId}) and CDN paths.
/// Slug: human-readable URL segment — unique, indexed.
/// Trailers are public; full video URL access is gated by the playback service.
/// </summary>
public class Movie
{
    public int Id { get; set; }
    public Guid PublicId { get; set; } = SequentialGuid.NewId();

    public string Title { get; set; } = default!;
    public string Slug { get; set; } = default!;
    public string? Description { get; set; }
    public string? ThumbnailUrl { get; set; }
    public string TrailerUrl { get; set; } = default!;
    public string VideoUrl { get; set; } = default!;   // CDN path — signed at request time
    public int DurationSeconds { get; set; }
    public int ReleaseYear { get; set; }
    public decimal AverageRating { get; set; }
    public bool IsPublished { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }

    // Navigation
    public ICollection<MovieGenre> MovieGenres { get; set; } = [];
    public ICollection<WatchHistory> WatchHistories { get; set; } = [];
}
