namespace StreamingApp.Domain.Entities;
/// <summary>
/// Aggregated daily search count per term. Upserted by a background job that
/// reads SearchLog and increments the count per (Term, Date).
/// Composite PK: (Term, Date) — one row per term per day, no surrogate needed.
/// </summary>
public class SearchTrend
{
    public string Term { get; set; } = default!;
    public DateOnly Date { get; set; }
    public long SearchCount { get; set; }
    public DateTime UpdatedAt { get; set; }
}
