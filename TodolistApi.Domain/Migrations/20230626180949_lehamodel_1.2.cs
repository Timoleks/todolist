using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TodolistApi.Domain.Migrations
{
    /// <inheritdoc />
    public partial class lehamodel_12 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "DayId",
                table: "TodoItems",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Days",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Date = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    UserID = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Days", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TodoItems_DayId",
                table: "TodoItems",
                column: "DayId");

            migrationBuilder.AddForeignKey(
                name: "FK_TodoItems_Days_DayId",
                table: "TodoItems",
                column: "DayId",
                principalTable: "Days",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TodoItems_Days_DayId",
                table: "TodoItems");

            migrationBuilder.DropTable(
                name: "Days");

            migrationBuilder.DropIndex(
                name: "IX_TodoItems_DayId",
                table: "TodoItems");

            migrationBuilder.DropColumn(
                name: "DayId",
                table: "TodoItems");
        }
    }
}
