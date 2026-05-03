namespace StreamingApp.Domain.Entities;

/// <summary>
/// Many-to-many join between Movie and Genre.
/// Composite PK (MovieId, GenreId) — both are int FKs.
/// </summary>
public class MovieGenre
{
    public int MovieId { get; set; }
    public int GenreId { get; set; }

    public Movie Movie { get; set; } = default!;
    public Genre Genre { get; set; } = default!;
}
