namespace StreamingApp.Domain.Common;

/// <summary>
/// Produces time-ordered UUID v7 values (built into .NET 9 via Guid.CreateVersion7).
/// UUID v7 encodes a millisecond-precision timestamp in the high bits, so rows
/// insert in chronological order into the clustered index with no page splits —
/// same write performance as a bigint IDENTITY column.
///
/// Use this in entity constructors for externally-visible token/session keys:
///   RefreshToken.Id, DeviceSession.Id, Download.Id
///
/// All FK columns reference int or long PKs — never Guid.
/// </summary>
public static class SequentialGuid
{
    public static Guid NewId() => Guid.CreateVersion7();

    /// <summary>Stamped to a specific instant — useful for deterministic test ordering.</summary>
    public static Guid NewId(DateTimeOffset timestamp) => Guid.CreateVersion7(timestamp);
}
