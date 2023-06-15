using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TodolistApi.Domain.Migrations
{
    /// <inheritdoc />
    public partial class lehamodel_11 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UserID",
                table: "TodoItems",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UserID",
                table: "TodoItems");
        }
    }
}
