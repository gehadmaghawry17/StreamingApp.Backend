using StreamingApp.Domain.Common;
using StreamingApp.Domain.Entities.Userss;

namespace StreamingApp.Domain.Entities;

/// <summary>
/// PK: int identity (clustered) — used for all internal FK joins.
/// PublicId: Guid v7 — exposed in JWTs, API responses, and URLs to prevent enumeration.
/// </summary>
public class User
{
    public int Id { get; set; }
    public Guid PublicId { get; set; } = SequentialGuid.NewId();

    public string Email { get; set; } = default!;
    public string PasswordHash { get; set; } = default!;
    public bool IsEmailVerified { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }

    // Navigation
    public ICollection<Favorite> Favorites { get; set; } = [];
    public ICollection<Rating> Ratings { get; set; } = [];
    public ICollection<Review> Reviews { get; set; } = [];
    public ICollection<ViewingProgress> ViewingProgresses { get; set; } = [];
    public ICollection<WatchHistory> WatchHistories { get; set; } = [];
    public ICollection<Subscription> Subscriptions { get; set; } = [];
    public ICollection<Payment> Payments { get; set; } = [];
    public ICollection<SearchLog> SearchLogs { get; set; } = [];
    public ICollection<Download> Downloads { get; set; } = [];
    public ICollection<DeviceSession> DeviceSessions { get; set; } = [];
    public ICollection<Otp> Otps { get; set; } = [];
    public ICollection<RefreshToken> RefreshTokens { get; set; } = [];
}
