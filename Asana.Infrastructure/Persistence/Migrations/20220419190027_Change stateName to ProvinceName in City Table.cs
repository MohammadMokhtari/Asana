using Microsoft.EntityFrameworkCore.Migrations;

namespace Asana.Infrastructure.Persistence.Migrations
{
    public partial class ChangestateNametoProvinceNameinCityTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Cities_Provinces_StateName",
                table: "Cities");

            migrationBuilder.RenameColumn(
                name: "StateName",
                table: "Cities",
                newName: "ProvinceName");

            migrationBuilder.RenameIndex(
                name: "IX_Cities_StateName",
                table: "Cities",
                newName: "IX_Cities_ProvinceName");

            migrationBuilder.AddForeignKey(
                name: "FK_Cities_Provinces_ProvinceName",
                table: "Cities",
                column: "ProvinceName",
                principalTable: "Provinces",
                principalColumn: "Name",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Cities_Provinces_ProvinceName",
                table: "Cities");

            migrationBuilder.RenameColumn(
                name: "ProvinceName",
                table: "Cities",
                newName: "StateName");

            migrationBuilder.RenameIndex(
                name: "IX_Cities_ProvinceName",
                table: "Cities",
                newName: "IX_Cities_StateName");

            migrationBuilder.AddForeignKey(
                name: "FK_Cities_Provinces_StateName",
                table: "Cities",
                column: "StateName",
                principalTable: "Provinces",
                principalColumn: "Name",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
