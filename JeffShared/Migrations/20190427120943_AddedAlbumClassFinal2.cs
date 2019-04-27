using Microsoft.EntityFrameworkCore.Migrations;

namespace JeffShared.Migrations
{
    public partial class AddedAlbumClassFinal2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "FileName",
                table: "Album",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FileName",
                table: "Album");
        }
    }
}
