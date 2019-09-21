using Microsoft.EntityFrameworkCore.Migrations;

namespace Data.Migrations
{
    public partial class Mig3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BookStatus",
                table: "Books");

            migrationBuilder.AddColumn<int>(
                name: "BookStatus",
                table: "StudentBooks",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "StudentBooks",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_StudentBooks_UserId",
                table: "StudentBooks",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_StudentBooks_AspNetUsers_UserId",
                table: "StudentBooks",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_StudentBooks_AspNetUsers_UserId",
                table: "StudentBooks");

            migrationBuilder.DropIndex(
                name: "IX_StudentBooks_UserId",
                table: "StudentBooks");

            migrationBuilder.DropColumn(
                name: "BookStatus",
                table: "StudentBooks");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "StudentBooks");

            migrationBuilder.AddColumn<int>(
                name: "BookStatus",
                table: "Books",
                nullable: false,
                defaultValue: 0);
        }
    }
}
