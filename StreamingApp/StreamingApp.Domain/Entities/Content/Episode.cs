namespace StreamingApp.Domain.Entities;

/// <summary>
/// PK: int identity. Internal entity — always queried via SeasonId + EpisodeNumber.
/// EpisodeNumber is 1-based and scoped to the season.
/// The playback service resolves the global episode index at runtime
/// (ordered by SeasonNumber then EpisodeNumber) to enforce the "first 2 free" rule.
/// VideoUrl is a CDN path — access is gated by the playback service.
/// </summary>
public class Episode
{
    public int Id { get; set; }
    public int SeasonId { get; set; }   // int FK → Season.Id

    public int EpisodeNumber { get; set; }
    public string Title { get; set; } = default!;
    public string? Description { get; set; }
    public string VideoUrl { get; set; } = default!;
    public int DurationSeconds { get; set; }
    public DateTime CreatedAt { get; set; }

    // Navigation
    public Season Season { get; set; } = default!;
    public ICollection<WatchHistory> WatchHistories { get; set; } = [];
}
