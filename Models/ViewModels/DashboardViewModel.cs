namespace MyStadiumStats.Models.ViewModels;

public class DashboardViewModel
{
    public Team? MyTeam { get; set; }
    public List<FollowedTeamStat> FollowedTeams { get; set; } = [];
    
    public int TotalMatches { get; set; }
    public int HomeWins { get; set; }
    public int AwayWins { get; set; }
    public int Draws { get; set; }
    public int Losses { get; set; }
    public int OtherResults { get; set; }
    
    public int TotalWins => HomeWins + AwayWins;
    public double WinRate => TotalMatches > 0 ? Math.Round((double)TotalWins / TotalMatches * 100, 1) : 0;
    public int TotalGoalsWitnessed { get; set; }

    public List<Match> RecentMatches { get; set; } = [];
    public List<PlayerGoalStat> TopScorers { get; set; } = [];
    public Dictionary<string, int> GoalsByMatchType { get; set; } = [];
    public Dictionary<string, int> MatchesByType { get; set; } = [];

    public class FollowedTeamStat
    {
        public Team Team { get; set; } = null!;
        public int Wins { get; set; }
        public int Draws { get; set; }
        public int Losses { get; set; }
        public int TotalMatches { get; set; }
        public List<Match> RecentMatches { get; set; } = [];
    }
}

public class PlayerGoalStat
{
    public int PlayerId { get; set; }
    public string PlayerName { get; set; } = string.Empty;
    public string? TeamName { get; set; }
    public int TotalGoals { get; set; }
    public int PenaltyGoals { get; set; }
    public double GoalPercentage { get; set; }
    public List<int> JerseyNumbers { get; set; } = [];
    public int? CurrentJerseyNumber { get; set; }
    public List<GoalBreakdown> Breakdown { get; set; } = [];

    public class GoalBreakdown
    {
        public string? TeamName { get; set; }
        public int? JerseyNumber { get; set; }
        public int Goals { get; set; }
        public int PenaltyGoals { get; set; }
    }
}
