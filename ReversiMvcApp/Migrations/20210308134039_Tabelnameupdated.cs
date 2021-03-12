using Microsoft.EntityFrameworkCore.Migrations;

namespace ReversiMvcApp.Migrations
{
    public partial class Tabelnameupdated : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Spel",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Token = table.Column<string>(nullable: true),
                    Speler1Token = table.Column<string>(nullable: true),
                    Speker2Token = table.Column<string>(nullable: true),
                    Omschrijving = table.Column<string>(nullable: true),
                    Winnaar = table.Column<string>(nullable: true),
                    Finished = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Spel", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Spelers",
                columns: table => new
                {
                    Guid = table.Column<string>(nullable: false),
                    Naam = table.Column<string>(nullable: true),
                    AantalGewonnen = table.Column<int>(nullable: false),
                    AantalVerloren = table.Column<int>(nullable: false),
                    AantalGelijk = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Spelers", x => x.Guid);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Spel");

            migrationBuilder.DropTable(
                name: "Spelers");
        }
    }
}
