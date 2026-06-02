using System.ComponentModel.DataAnnotations;

namespace MyStadiumStats.Models.ViewModels;

public class MatchFormViewModel
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Tarih gerekli")]
    [DataType(DataType.Date)]
    [Display(Name = "Tarih")]
    public DateTime Date { get; set; } = DateTime.Today;

    [Required(ErrorMessage = "Ev sahibi takım gerekli")]
    [Display(Name = "Ev Sahibi")]
    public string HomeTeamName { get; set; } = string.Empty;

    [Required(ErrorMessage = "Deplasman takımı gerekli")]
    [Display(Name = "Deplasman")]
    public string AwayTeamName { get; set; } = string.Empty;

    [Required(ErrorMessage = "Stadyum gerekli")]
    [Display(Name = "Stadyum")]
    public string StadiumName { get; set; } = string.Empty;

    [Display(Name = "Ev Sahibi Skoru")]
    [Range(0, 99)]
    public int HomeScore { get; set; }

    [Display(Name = "Deplasman Skoru")]
    [Range(0, 99)]
    public int AwayScore { get; set; }

    [Required(ErrorMessage = "Maç türü gerekli")]
    [Display(Name = "Maç Türü")]
    public string MatchType { get; set; } = string.Empty;

    [Display(Name = "Ev Sahibi Teknik Direktör")]
    public string? HomeCoachName { get; set; }

    [Display(Name = "Deplasman Teknik Direktör")]
    public string? AwayCoachName { get; set; }

    [Display(Name = "Notlar")]
    public string? Notes { get; set; }

    // Goals as JSON (serialized from JS, deserialized in controller)
    public string GoalsJson { get; set; } = "[]";

    // Reference data for autocomplete datalists (populated by controller)
    public List<string> TeamNames { get; set; } = [];
    public List<string> StadiumNames { get; set; } = [];
    public List<string> CoachNames { get; set; } = [];
    public IEnumerable<Player> Players { get; set; } = [];
}

public class GoalInput
{
    public string PlayerName { get; set; } = string.Empty;
    public int? JerseyNumber { get; set; }
    public int Minute { get; set; }
    public bool IsOwnGoal { get; set; }
    public bool IsPenalty { get; set; }
    public string TeamSide { get; set; } = "home"; // "home" or "away"
}
