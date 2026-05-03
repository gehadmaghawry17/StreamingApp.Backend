using StreamingApp.Domain.Common;

namespace StreamingApp.Domain.Entities;
/// <summary>
/// Persisted refresh token used to issue new access tokens.
/// PK: Guid v7 — externally issued token must be opaque and unguessable.
/// Sequential bits keep clustered inserts ordered.
/// The raw token is NEVER stored — only its SHA-256 hash.
/// Revocation lookup uses the unique index on TokenHash.
/// </summary>
public class RefreshToken
{
    public Guid Id { get; set; } = SequentialGuid.NewId();

    public int UserId { get; set; }   // int FK → User.Id

    public string TokenHash { get; set; } = default!;
    public DateTime ExpiresAt { get; set; }
    public bool IsRevoked { get; set; }
    public DateTime CreatedAt { get; set; }

    // Navigation
    public User User { get; set; } = default!;
}
