using System.ComponentModel.DataAnnotations;

namespace MyStadiumStats.Models;

public class Player
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Oyuncu adı gerekli")]
    [Display(Name = "Ad Soyad")]
    public string Name { get; set; } = string.Empty;

    [Display(Name = "Güncel Forma No")]
    public int? CurrentJerseyNumber { get; set; }

    public ICollection<Goal> Goals { get; set; } = [];
}
