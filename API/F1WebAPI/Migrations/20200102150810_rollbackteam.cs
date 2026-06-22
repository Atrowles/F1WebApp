using Microsoft.EntityFrameworkCore.Migrations;

namespace F1WebAPI.Migrations
{
    public partial class rollbackteam : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TeamName",
                table: "Drivers");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "TeamName",
                table: "Drivers",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
