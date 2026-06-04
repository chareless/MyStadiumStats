using System.ComponentModel.DataAnnotations;

namespace MyStadiumStats.Models;

public class Team
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Takım adı gerekli")]
    [Display(Name = "Takım Adı")]
    public string Name { get; set; } = string.Empty;

    [Display(Name = "Ülke")]
    public string? Country { get; set; }

    [Display(Name = "Benim Takımım")]
    public bool IsMyTeam { get; set; }

    [Display(Name = "Takip Et")]
    public bool IsFollowed { get; set; }

    public ICollection<Match> HomeMatches { get; set; } = [];
    public ICollection<Match> AwayMatches { get; set; } = [];
    public ICollection<Goal> GoalsForTeam { get; set; } = [];
}
