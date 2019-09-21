using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Data.Migrations
{
    public partial class Mig5 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "BorrowDate",
                table: "ReserveBooks",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ReserveDate",
                table: "ReserveBooks",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BorrowDate",
                table: "ReserveBooks");

            migrationBuilder.DropColumn(
                name: "ReserveDate",
                table: "ReserveBooks");
        }
    }
}
