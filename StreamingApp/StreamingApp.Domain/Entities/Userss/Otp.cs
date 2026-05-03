using StreamingApp.Domain.Enums;

namespace StreamingApp.Domain.Entities.Userss;

/// <summary>
/// Short-lived OTP record — created on every login/reset attempt, deleted on use or expiry.
/// PK: long (bigint) identity — high frequency inserts, rows are ephemeral.
/// The raw code is NEVER stored — only its hash. Lookup is by CodeHash on a tiny table.
/// No indexes beyond the PK: a background cleanup job keeps the table small.
/// </summary>
public class Otp
{
    public long Id { get; set; }

    public int UserId { get; set; }   // int FK → User.Id
    public string CodeHash { get; set; } = default!;
    public OtpType Type { get; set; }
    public DateTime ExpiresAt { get; set; }
    public bool IsUsed { get; set; }
    public DateTime CreatedAt { get; set; }

    // Navigation
    public User User { get; set; } = default!;
}
