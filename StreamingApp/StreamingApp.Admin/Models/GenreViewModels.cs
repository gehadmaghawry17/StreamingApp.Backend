using System.ComponentModel.DataAnnotations;

namespace StreamingApp.Admin.Models;

public sealed class GenreFormViewModel
{
    public int Id { get; set; }

    [Required]
    [StringLength(64)]
    public string Name { get; set; } = string.Empty;
}
