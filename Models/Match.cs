using System.ComponentModel.DataAnnotations;

namespace MyStadiumStats.Models;

public class Match
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Tarih gerekli")]
    [DataType(DataType.Date)]
    [Display(Name = "Tarih")]
    public DateTime Date { get; set; }

    [Required(ErrorMessage = "Ev sahibi takım gerekli")]
    public int HomeTeamId { get; set; }

    [Required(ErrorMessage = "Deplasman takımı gerekli")]
    public int AwayTeamId { get; set; }

    [Required(ErrorMessage = "Stadyum gerekli")]
    public int StadiumId { get; set; }

    [Display(Name = "Ev Sahibi Skoru")]
    [Range(0, 99, ErrorMessage = "Skor 0-99 arasında olmalı")]
    public int HomeScore { get; set; }

    [Display(Name = "Deplasman Skoru")]
    [Range(0, 99, ErrorMessage = "Skor 0-99 arasında olmalı")]
    public int AwayScore { get; set; }

    [Required(ErrorMessage = "Maç türü gerekli")]
    [Display(Name = "Maç Türü")]
    public string MatchType { get; set; } = string.Empty;

    [Display(Name = "Ev Sahibi Teknik Direktör")]
    public int? HomeCoachId { get; set; }

    [Display(Name = "Deplasman Teknik Direktör")]
    public int? AwayCoachId { get; set; }

    [Display(Name = "Notlar")]
    public string? Notes { get; set; }

    // Navigation properties
    public Team HomeTeam { get; set; } = null!;
    public Team AwayTeam { get; set; } = null!;
    public Stadium Stadium { get; set; } = null!;
    public Coach? HomeCoach { get; set; }
    public Coach? AwayCoach { get; set; }
    public ICollection<Goal> Goals { get; set; } = [];
}
