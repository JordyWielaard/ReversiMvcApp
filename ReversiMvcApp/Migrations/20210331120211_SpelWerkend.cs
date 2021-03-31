using Microsoft.EntityFrameworkCore.Migrations;

namespace ReversiMvcApp.Migrations
{
    public partial class SpelWerkend : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Finished",
                table: "Spel");

            migrationBuilder.DropColumn(
                name: "Speker2Token",
                table: "Spel");

            migrationBuilder.AddColumn<bool>(
                name: "Afgelopen",
                table: "Spel",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "Speler2Token",
                table: "Spel",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Afgelopen",
                table: "Spel");

            migrationBuilder.DropColumn(
                name: "Speler2Token",
                table: "Spel");

            migrationBuilder.AddColumn<bool>(
                name: "Finished",
                table: "Spel",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "Speker2Token",
                table: "Spel",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
