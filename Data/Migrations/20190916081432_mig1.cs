using Microsoft.EntityFrameworkCore.Migrations;

namespace Data.Migrations
{
    public partial class mig1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Teachers_Fields_FieldId",
                table: "Teachers");

            migrationBuilder.DropIndex(
                name: "IX_Teachers_FieldId",
                table: "Teachers");

            migrationBuilder.DropColumn(
                name: "FieldId",
                table: "Teachers");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "FieldId",
                table: "Teachers",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Teachers_FieldId",
                table: "Teachers",
                column: "FieldId");

            migrationBuilder.AddForeignKey(
                name: "FK_Teachers_Fields_FieldId",
                table: "Teachers",
                column: "FieldId",
                principalTable: "Fields",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
