namespace MyStadiumStats.Models;

public class Goal
{
    public int Id { get; set; }
    public int MatchId { get; set; }
    public int PlayerId { get; set; }
    public int? TeamIdAtMatch { get; set; }
    public int Minute { get; set; }
    public bool IsOwnGoal { get; set; }
    public bool IsPenalty { get; set; }
    public int? JerseyNumberAtMatch { get; set; }

    public Match Match { get; set; } = null!;
    public Player Player { get; set; } = null!;
    public Team? TeamAtMatch { get; set; }
}
