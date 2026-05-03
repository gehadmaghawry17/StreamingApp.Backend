namespace StreamingApp.Domain.Entities;
/// <summary>
/// Raw search event log — every search term typed by any user appends a row.
/// PK: long (bigint) identity — high insert rate, append-sequential, no page splits.
/// UserId is nullable — unauthenticated searches are still logged.
/// Do NOT index Term here; aggregate into SearchTrend via a background job instead.
/// </summary>
public class SearchLog
{
    public long Id { get; set; }

    public int? UserId { get; set; }   // nullable int FK → User.Id
    public string Term { get; set; } = default!;
    public DateTime CreatedAt { get; set; }

    // Navigation
    public User? User { get; set; }
}
