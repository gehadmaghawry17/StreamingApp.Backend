using StreamingApp.Domain.Common;
using StreamingApp.Domain.Enums;

namespace StreamingApp.Domain.Entities;

/// <summary>
/// Offline download token. Out of MVP scope but schema is pre-defined.
/// PK: Guid v7 — the download token is shared with CDN/clients and must be
/// unguessable. Sequential Guid keeps clustered inserts ordered.
///
/// XOR: exactly one of MovieId or EpisodeId is non-null per row.
/// </summary>
public class Download
{
    public Guid Id { get; set; } = SequentialGuid.NewId();

    public int UserId { get; set; }       // int FK → User.Id
    public int? MovieId { get; set; }     // int FK → Movie.Id
    public int? EpisodeId { get; set; }   // int FK → Episode.Id

    public DateTime ExpiresAt { get; set; }
    public bool IsRevoked { get; set; }
    public DateTime CreatedAt { get; set; }

    // Navigation
    public User User { get; set; } = default!;
    public Movie? Movie { get; set; }
    public Episode? Episode { get; set; }
}
