using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Asana.Infrastructure.Persistence.Migrations
{
    public partial class RenameStateTabletoProvince : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Addresses_States_StateName",
                table: "Addresses");

            migrationBuilder.DropForeignKey(
                name: "FK_Cities_States_StateName",
                table: "Cities");

            migrationBuilder.DropTable(
                name: "States");

            migrationBuilder.RenameColumn(
                name: "StateName",
                table: "Addresses",
                newName: "ProvinceName");

            migrationBuilder.RenameIndex(
                name: "IX_Addresses_StateName",
                table: "Addresses",
                newName: "IX_Addresses_ProvinceName");

            migrationBuilder.CreateTable(
                name: "Provinces",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    IsDelete = table.Column<bool>(type: "bit", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Provinces", x => x.Id);
                    table.UniqueConstraint("AK_Provinces_Name", x => x.Name);
                });

            migrationBuilder.AddForeignKey(
                name: "FK_Addresses_Provinces_ProvinceName",
                table: "Addresses",
                column: "ProvinceName",
                principalTable: "Provinces",
                principalColumn: "Name",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Cities_Provinces_StateName",
                table: "Cities",
                column: "StateName",
                principalTable: "Provinces",
                principalColumn: "Name",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Addresses_Provinces_ProvinceName",
                table: "Addresses");

            migrationBuilder.DropForeignKey(
                name: "FK_Cities_Provinces_StateName",
                table: "Cities");

            migrationBuilder.DropTable(
                name: "Provinces");

            migrationBuilder.RenameColumn(
                name: "ProvinceName",
                table: "Addresses",
                newName: "StateName");

            migrationBuilder.RenameIndex(
                name: "IX_Addresses_ProvinceName",
                table: "Addresses",
                newName: "IX_Addresses_StateName");

            migrationBuilder.CreateTable(
                name: "States",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsDelete = table.Column<bool>(type: "bit", nullable: false),
                    ModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_States", x => x.Id);
                    table.UniqueConstraint("AK_States_Name", x => x.Name);
                });

            migrationBuilder.AddForeignKey(
                name: "FK_Addresses_States_StateName",
                table: "Addresses",
                column: "StateName",
                principalTable: "States",
                principalColumn: "Name",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Cities_States_StateName",
                table: "Cities",
                column: "StateName",
                principalTable: "States",
                principalColumn: "Name",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
