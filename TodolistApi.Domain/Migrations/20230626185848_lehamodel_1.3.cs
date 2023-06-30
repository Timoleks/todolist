using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TodolistApi.Domain.Migrations
{
    /// <inheritdoc />
    public partial class lehamodel_13 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UserID",
                table: "Days");

            migrationBuilder.RenameColumn(
                name: "UserID",
                table: "TodoItems",
                newName: "UserId");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "TodoItems",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.CreateIndex(
                name: "IX_TodoItems_UserId",
                table: "TodoItems",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_TodoItems_AspNetUsers_UserId",
                table: "TodoItems",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TodoItems_AspNetUsers_UserId",
                table: "TodoItems");

            migrationBuilder.DropIndex(
                name: "IX_TodoItems_UserId",
                table: "TodoItems");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "TodoItems",
                newName: "UserID");

            migrationBuilder.AlterColumn<string>(
                name: "UserID",
                table: "TodoItems",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UserID",
                table: "Days",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
