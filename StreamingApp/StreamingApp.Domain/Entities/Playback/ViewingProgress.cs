using StreamingApp.Domain.Enums;

namespace StreamingApp.Domain.Entities;

/// <summary>
/// Tracks per-user playback progress for Continue Watching.
/// Composite PK: (UserId int, ContentType enum, ContentId int).
/// ContentType: Movie or Episode (progress is per-episode for series, per-movie for films).
///
/// DurationSeconds is denormalised from the source content at write time so that
/// progress percentage can be computed entirely in SQL without a join:
///   ProgressSeconds * 100.0 / DurationSeconds
///
/// Business rules:
///   Continue Watching: ProgressPercent > 5 AND ProgressPercent &lt; 90
///   Completed        : ProgressPercent &gt;= 90  (IsCompleted = true, set by app layer)
/// </summary>
public class ViewingProgress
{
    public int UserId { get; set; }
    public ContentType ContentType { get; set; }
    public int ContentId { get; set; }

    public int ProgressSeconds { get; set; }

    /// <summary>
    /// Denormalised from Movie.DurationSeconds or Episode.DurationSeconds at write time.
    /// </summary>
    public int DurationSeconds { get; set; }

    /// <summary>True when ProgressSeconds / DurationSeconds &gt;= 0.90. Set by the app layer.</summary>
    public bool IsCompleted { get; set; }

    public DateTime LastWatchedAt { get; set; }
    public DateTime CreatedAt { get; set; }

    // Navigation
    public User User { get; set; } = default!;
}
