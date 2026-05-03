namespace StreamingApp.Domain.Entities;

/// <summary>
/// PK: int identity. Internal entity — always queried via SeriesId + SeasonNumber.
/// No PublicId needed; the parent Series.PublicId identifies the content externally.
/// </summary>
public class Season
{
    public int Id { get; set; }
    public int SeriesId { get; set; }   // int FK → Series.Id

    public int SeasonNumber { get; set; }
    public string Title { get; set; } = default!;
    public DateTime CreatedAt { get; set; }

    // Navigation
    public Series Series { get; set; } = default!;
    public ICollection<Episode> Episodes { get; set; } = [];
}
