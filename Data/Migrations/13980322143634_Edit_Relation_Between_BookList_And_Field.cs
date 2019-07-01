using Microsoft.EntityFrameworkCore.Migrations;

namespace Data.Migrations
{
    public partial class Edit_Relation_Between_BookList_And_Field : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Books_Fields_FieldId",
                table: "Books");

            migrationBuilder.DropIndex(
                name: "IX_Books_FieldId",
                table: "Books");

            migrationBuilder.DropColumn(
                name: "FieldId",
                table: "Books");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "FieldId",
                table: "Books",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Books_FieldId",
                table: "Books",
                column: "FieldId");

            migrationBuilder.AddForeignKey(
                name: "FK_Books_Fields_FieldId",
                table: "Books",
                column: "FieldId",
                principalTable: "Fields",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
