namespace StreamingApp.Domain.Enums;

/// <summary>
/// Discriminator for polymorphic tables: Favorite, Rating, ViewingProgress, Review.
/// Stored as a string column via HasConversion&lt;string&gt;() in each configuration.
/// EF Core does NOT map enums as tables — no HasNoKey() or modelBuilder.Ignore needed.
/// </summary>
public enum ContentType
{
    Movie,
    Series,
    Episode   // used by ViewingProgress — progress is tracked per episode, not per series
}
