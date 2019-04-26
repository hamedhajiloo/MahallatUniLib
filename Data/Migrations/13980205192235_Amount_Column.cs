using Microsoft.EntityFrameworkCore.Migrations;

namespace Data.Migrations
{
    public partial class Amount_Column : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "PenaltyAmount",
                table: "Students",
                nullable: false,
                defaultValue: 0m);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PenaltyAmount",
                table: "Students");
        }
    }
}
