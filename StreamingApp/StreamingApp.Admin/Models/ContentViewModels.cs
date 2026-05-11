using System.ComponentModel.DataAnnotations;

namespace StreamingApp.Admin.Models;

public sealed class GenreCheckboxViewModel
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public bool IsSelected { get; set; }
}

public sealed class MovieListItemViewModel
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Slug { get; set; } = string.Empty;
    public int ReleaseYear { get; set; }
    public bool IsPublished { get; set; }
    public string Genres { get; set; } = string.Empty;
}

public sealed class MovieFormViewModel
{
    public int Id { get; set; }

    [Required]
    [StringLength(256)]
    public string Title { get; set; } = string.Empty;

    [StringLength(300)]
    public string? Slug { get; set; }

    [Display(Name = "Description")]
    [StringLength(2000)]
    public string? Description { get; set; }

    [Display(Name = "Thumbnail URL")]
    [StringLength(1024)]
    public string? ThumbnailUrl { get; set; }

    [Display(Name = "Trailer URL")]
    [Required]
    [StringLength(1024)]
    public string TrailerUrl { get; set; } = string.Empty;

    [Display(Name = "Video URL")]
    [Required]
    [StringLength(1024)]
    public string VideoUrl { get; set; } = string.Empty;

    [Display(Name = "Duration Seconds")]
    [Range(1, int.MaxValue)]
    public int DurationSeconds { get; set; }

    [Display(Name = "Release Year")]
    [Range(1888, 3000)]
    public int ReleaseYear { get; set; } = DateTime.UtcNow.Year;

    [Display(Name = "Is Published")]
    public bool IsPublished { get; set; }
    public List<int> SelectedGenreIds { get; set; } = [];
    public List<GenreCheckboxViewModel> Genres { get; set; } = [];
}

public sealed class SeriesListItemViewModel
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Slug { get; set; } = string.Empty;
    public bool IsPublished { get; set; }
    public string Genres { get; set; } = string.Empty;
}

public sealed class SeriesFormViewModel
{
    public int Id { get; set; }

    [Required]
    [StringLength(256)]
    public string Title { get; set; } = string.Empty;

    [StringLength(300)]
    public string? Slug { get; set; }

    [Display(Name = "Description")]
    [StringLength(2000)]
    public string? Description { get; set; }

    [Display(Name = "Thumbnail URL")]
    [StringLength(1024)]
    public string? ThumbnailUrl { get; set; }

    [Display(Name = "Trailer URL")]
    [Required]
    [StringLength(1024)]
    public string TrailerUrl { get; set; } = string.Empty;

    [Display(Name = "Is Published")]
    public bool IsPublished { get; set; }
    public List<int> SelectedGenreIds { get; set; } = [];
    public List<GenreCheckboxViewModel> Genres { get; set; } = [];
}
