using Microsoft.EntityFrameworkCore.Migrations;

namespace Asana.Infrastructure.Persistence.Migrations
{
    public partial class AddedGenderInUserApplication : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "GenderId",
                table: "Users",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "GenderId",
                table: "Users");
        }
    }
}
