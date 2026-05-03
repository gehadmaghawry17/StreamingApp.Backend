namespace StreamingApp.Domain.Entities;

/// <summary>
/// PK: int identity. Tiny seeded lookup table — int is readable in join tables
/// and has zero performance overhead at this cardinality.
/// </summary>
public class Genre
{
    public int Id { get; set; }
    public string Name { get; set; } = default!;

    // Navigation
    public ICollection<MovieGenre> MovieGenres { get; set; } = [];
    public ICollection<SeriesGenre> SeriesGenres { get; set; } = [];
}
