using System.ComponentModel.DataAnnotations;

namespace MyStadiumStats.Models;

public class Coach
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Teknik direktör adı gerekli")]
    [Display(Name = "Ad Soyad")]
    public string Name { get; set; } = string.Empty;

    public ICollection<Match> HomeCoachMatches { get; set; } = [];
    public ICollection<Match> AwayCoachMatches { get; set; } = [];
}
