namespace StreamingApp.Domain.Entities;

/// <summary>
/// Many-to-many join between Series and Genre.
/// Composite PK (SeriesId, GenreId) — both are int FKs.
/// </summary>
public class SeriesGenre
{
    public int SeriesId { get; set; }
    public int GenreId { get; set; }

    public Series Series { get; set; } = default!;
    public Genre Genre { get; set; } = default!;
}
