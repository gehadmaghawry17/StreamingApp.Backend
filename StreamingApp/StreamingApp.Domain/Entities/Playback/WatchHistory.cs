namespace StreamingApp.Domain.Entities;

/// <summary>
/// Append-only play-event log — every playback session writes a row.
/// PK: long (bigint) identity — sequential inserts, prevents overflow over platform lifetime.
///
/// XOR constraint: exactly one of MovieId or EpisodeId is non-null per row.
/// Enforced at DB level via a CHECK constraint in WatchHistoryConfiguration.
/// </summary>
public class WatchHistory
{
    public long Id { get; set; }

    public int UserId { get; set; }         // int FK → User.Id
    public int? MovieId { get; set; }       // int FK → Movie.Id   (null when EpisodeId is set)
    public int? EpisodeId { get; set; }     // int FK → Episode.Id (null when MovieId is set)

    public int WatchedSeconds { get; set; }
    public DateTime WatchedAt { get; set; }

    // Navigation
    public User User { get; set; } = default!;
    public Movie? Movie { get; set; }
    public Episode? Episode { get; set; }
}
