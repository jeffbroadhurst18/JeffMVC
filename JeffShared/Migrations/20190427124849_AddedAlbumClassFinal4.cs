using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace JeffShared.Migrations
{
    public partial class AddedAlbumClassFinal4 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Name",
                table: "MemberAlbum");

            migrationBuilder.AddColumn<int>(
                name: "AlbumId",
                table: "MemberAlbum",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "MemberId",
                table: "MemberAlbum",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Member",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Member", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MemberAlbum_AlbumId",
                table: "MemberAlbum",
                column: "AlbumId");

            migrationBuilder.CreateIndex(
                name: "IX_MemberAlbum_MemberId",
                table: "MemberAlbum",
                column: "MemberId");

            migrationBuilder.AddForeignKey(
                name: "FK_MemberAlbum_Album_AlbumId",
                table: "MemberAlbum",
                column: "AlbumId",
                principalTable: "Album",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_MemberAlbum_Member_MemberId",
                table: "MemberAlbum",
                column: "MemberId",
                principalTable: "Member",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MemberAlbum_Album_AlbumId",
                table: "MemberAlbum");

            migrationBuilder.DropForeignKey(
                name: "FK_MemberAlbum_Member_MemberId",
                table: "MemberAlbum");

            migrationBuilder.DropTable(
                name: "Member");

            migrationBuilder.DropIndex(
                name: "IX_MemberAlbum_AlbumId",
                table: "MemberAlbum");

            migrationBuilder.DropIndex(
                name: "IX_MemberAlbum_MemberId",
                table: "MemberAlbum");

            migrationBuilder.DropColumn(
                name: "AlbumId",
                table: "MemberAlbum");

            migrationBuilder.DropColumn(
                name: "MemberId",
                table: "MemberAlbum");

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "MemberAlbum",
                nullable: true);
        }
    }
}
