using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MyStadiumStats.Migrations
{
    /// <inheritdoc />
    public partial class AddFollowedTeamAndUpdateModels : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsFollowed",
                table: "Teams",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsFollowed",
                table: "Teams");
        }
    }
}
