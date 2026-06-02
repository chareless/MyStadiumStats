using System.ComponentModel.DataAnnotations;

namespace MyStadiumStats.Models;

public class Stadium
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Stadyum adı gerekli")]
    [Display(Name = "Stadyum Adı")]
    public string Name { get; set; } = string.Empty;

    [Display(Name = "Şehir")]
    public string? City { get; set; }

    [Display(Name = "Ülke")]
    public string? Country { get; set; }

    public ICollection<Match> Matches { get; set; } = [];
}
