using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace F1WebAPI.Migrations
{
    public partial class fieldchange : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DriverResults",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ResultId = table.Column<long>(nullable: false),
                    DriverId = table.Column<string>(nullable: true),
                    Position = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DriverResults", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RaceResults",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TrackId = table.Column<long>(nullable: false),
                    FastestLapTime = table.Column<decimal>(type: "decimal(18,4)", nullable: false),
                    FastestLapDriver = table.Column<int>(nullable: false),
                    PoleTime = table.Column<decimal>(type: "decimal(18,4)", nullable: false),
                    PoleDriver = table.Column<int>(nullable: false),
                    RaceDate = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RaceResults", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DriverResults");

            migrationBuilder.DropTable(
                name: "RaceResults");
        }
    }
}
