using Microsoft.EntityFrameworkCore.Migrations;

namespace JeffShared.Migrations
{
    public partial class AddedAlbumClassFinal3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Member_Album_AlbumId",
                table: "Member");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Member",
                table: "Member");

            migrationBuilder.DropIndex(
                name: "IX_Member_AlbumId",
                table: "Member");

            migrationBuilder.DropColumn(
                name: "AlbumId",
                table: "Member");

            migrationBuilder.RenameTable(
                name: "Member",
                newName: "MemberAlbum");

            migrationBuilder.AddPrimaryKey(
                name: "PK_MemberAlbum",
                table: "MemberAlbum",
                column: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_MemberAlbum",
                table: "MemberAlbum");

            migrationBuilder.RenameTable(
                name: "MemberAlbum",
                newName: "Member");

            migrationBuilder.AddColumn<int>(
                name: "AlbumId",
                table: "Member",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Member",
                table: "Member",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_Member_AlbumId",
                table: "Member",
                column: "AlbumId");

            migrationBuilder.AddForeignKey(
                name: "FK_Member_Album_AlbumId",
                table: "Member",
                column: "AlbumId",
                principalTable: "Album",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
