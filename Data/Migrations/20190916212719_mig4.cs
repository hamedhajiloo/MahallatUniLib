using Microsoft.EntityFrameworkCore.Migrations;

namespace Data.Migrations
{
    public partial class mig4 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUserRoles_AspNetRoles_RoleId1",
                table: "AspNetUserRoles");

            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUserRoles_AspNetUsers_UserId1",
                table: "AspNetUserRoles");

            migrationBuilder.RenameColumn(
                name: "UserId1",
                table: "AspNetUserRoles",
                newName: "UserId2");

            migrationBuilder.RenameColumn(
                name: "RoleId1",
                table: "AspNetUserRoles",
                newName: "RoleId2");

            migrationBuilder.RenameIndex(
                name: "IX_AspNetUserRoles_UserId1",
                table: "AspNetUserRoles",
                newName: "IX_AspNetUserRoles_UserId2");

            migrationBuilder.RenameIndex(
                name: "IX_AspNetUserRoles_RoleId1",
                table: "AspNetUserRoles",
                newName: "IX_AspNetUserRoles_RoleId2");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUserRoles_AspNetRoles_RoleId2",
                table: "AspNetUserRoles",
                column: "RoleId2",
                principalTable: "AspNetRoles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUserRoles_AspNetUsers_UserId2",
                table: "AspNetUserRoles",
                column: "UserId2",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUserRoles_AspNetRoles_RoleId2",
                table: "AspNetUserRoles");

            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUserRoles_AspNetUsers_UserId2",
                table: "AspNetUserRoles");

            migrationBuilder.RenameColumn(
                name: "UserId2",
                table: "AspNetUserRoles",
                newName: "UserId1");

            migrationBuilder.RenameColumn(
                name: "RoleId2",
                table: "AspNetUserRoles",
                newName: "RoleId1");

            migrationBuilder.RenameIndex(
                name: "IX_AspNetUserRoles_UserId2",
                table: "AspNetUserRoles",
                newName: "IX_AspNetUserRoles_UserId1");

            migrationBuilder.RenameIndex(
                name: "IX_AspNetUserRoles_RoleId2",
                table: "AspNetUserRoles",
                newName: "IX_AspNetUserRoles_RoleId1");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUserRoles_AspNetRoles_RoleId1",
                table: "AspNetUserRoles",
                column: "RoleId1",
                principalTable: "AspNetRoles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUserRoles_AspNetUsers_UserId1",
                table: "AspNetUserRoles",
                column: "UserId1",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
