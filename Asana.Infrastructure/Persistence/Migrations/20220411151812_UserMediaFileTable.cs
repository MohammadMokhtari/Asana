using Microsoft.EntityFrameworkCore.Migrations;

namespace Asana.Infrastructure.Persistence.Migrations
{
    public partial class UserMediaFileTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MediaFiles_Users_UserId",
                table: "MediaFiles");

            migrationBuilder.DropPrimaryKey(
                name: "PK_MediaFiles",
                table: "MediaFiles");

            migrationBuilder.RenameTable(
                name: "MediaFiles",
                newName: "UserMediaFiles");

            migrationBuilder.RenameIndex(
                name: "IX_MediaFiles_UserId",
                table: "UserMediaFiles",
                newName: "IX_UserMediaFiles_UserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserMediaFiles",
                table: "UserMediaFiles",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_UserMediaFiles_Users_UserId",
                table: "UserMediaFiles",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserMediaFiles_Users_UserId",
                table: "UserMediaFiles");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserMediaFiles",
                table: "UserMediaFiles");

            migrationBuilder.RenameTable(
                name: "UserMediaFiles",
                newName: "MediaFiles");

            migrationBuilder.RenameIndex(
                name: "IX_UserMediaFiles_UserId",
                table: "MediaFiles",
                newName: "IX_MediaFiles_UserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_MediaFiles",
                table: "MediaFiles",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_MediaFiles_Users_UserId",
                table: "MediaFiles",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
