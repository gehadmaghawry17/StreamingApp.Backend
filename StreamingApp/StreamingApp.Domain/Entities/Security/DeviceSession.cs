using StreamingApp.Domain.Common;

namespace StreamingApp.Domain.Entities;
/// <summary>
/// Active device/session record. MVP allows one session per user — enforced
/// by a unique index on UserId in DeviceSessionConfiguration.
///
/// PK: Guid v7 — session tokens are externally visible in auth flows.
/// Sequential bits keep clustered inserts ordered without page splits.
/// All FK references (UserId, RefreshTokenId) use int/Guid of the PK type
/// of the target table — not a secondary Guid.
/// </summary>
public class DeviceSession
{
    public Guid Id { get; set; } = SequentialGuid.NewId();

    public int UserId { get; set; }     // int FK → User.Id

    public string? DeviceInfo { get; set; }
    public string? IpAddress { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime LastActiveAt { get; set; }

    // Navigation
    public User User { get; set; } = default!;
}
