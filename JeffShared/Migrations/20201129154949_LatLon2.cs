using Microsoft.EntityFrameworkCore.Migrations;

namespace JeffShared.Migrations
{
    public partial class LatLon2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "Lat",
                table: "Cities",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "Lon",
                table: "Cities",
                nullable: false,
                defaultValue: 0.0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Lat",
                table: "Cities");

            migrationBuilder.DropColumn(
                name: "Lon",
                table: "Cities");
        }
    }
}
