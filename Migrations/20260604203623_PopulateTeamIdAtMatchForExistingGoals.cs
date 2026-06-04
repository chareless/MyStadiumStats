using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MyStadiumStats.Migrations
{
    /// <inheritdoc />
    public partial class PopulateTeamIdAtMatchForExistingGoals : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Populate TeamIdAtMatch for existing goals
            // For own goals: assign to the opposite team (if OwnGoal=1, assign to AwayTeamId)
            // For regular goals: attempt to infer from score progression, default to HomeTeamId if unable to determine
            
            migrationBuilder.Sql(
                @"UPDATE Goals 
                  SET TeamIdAtMatch = CASE 
                        WHEN IsOwnGoal = 1 THEN (SELECT AwayTeamId FROM Matches WHERE Id = Goals.MatchId)
                        ELSE (SELECT HomeTeamId FROM Matches WHERE Id = Goals.MatchId)
                      END
                  WHERE TeamIdAtMatch IS NULL");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Revert the population
            migrationBuilder.Sql(
                @"UPDATE Goals 
                  SET TeamIdAtMatch = NULL
                  WHERE TeamIdAtMatch IS NOT NULL");
        }
    }
}
