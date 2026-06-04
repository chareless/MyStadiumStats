using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MyStadiumStats.Migrations
{
    /// <inheritdoc />
    public partial class RemoveTeamFromPlayerAddTeamToGoal : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Players_Teams_TeamId",
                table: "Players");

            migrationBuilder.DropIndex(
                name: "IX_Players_TeamId",
                table: "Players");

            migrationBuilder.DropColumn(
                name: "TeamId",
                table: "Players");

            migrationBuilder.AddColumn<int>(
                name: "TeamIdAtMatch",
                table: "Goals",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Goals_TeamIdAtMatch",
                table: "Goals",
                column: "TeamIdAtMatch");

            migrationBuilder.AddForeignKey(
                name: "FK_Goals_Teams_TeamIdAtMatch",
                table: "Goals",
                column: "TeamIdAtMatch",
                principalTable: "Teams",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Goals_Teams_TeamIdAtMatch",
                table: "Goals");

            migrationBuilder.DropIndex(
                name: "IX_Goals_TeamIdAtMatch",
                table: "Goals");

            migrationBuilder.DropColumn(
                name: "TeamIdAtMatch",
                table: "Goals");

            migrationBuilder.AddColumn<int>(
                name: "TeamId",
                table: "Players",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Players_TeamId",
                table: "Players",
                column: "TeamId");

            migrationBuilder.AddForeignKey(
                name: "FK_Players_Teams_TeamId",
                table: "Players",
                column: "TeamId",
                principalTable: "Teams",
                principalColumn: "Id");
        }
    }
}
