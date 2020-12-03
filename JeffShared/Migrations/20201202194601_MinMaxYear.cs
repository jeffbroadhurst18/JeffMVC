using Microsoft.EntityFrameworkCore.Migrations;

namespace JeffShared.Migrations
{
    public partial class MinMaxYear : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Year",
                table: "MinMaxTemps",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Year",
                table: "MinMaxTemps");
        }
    }
}
