using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataLayer.Migrations
{
    /// <inheritdoc />
    public partial class ChessGameUpdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "BlackPlayerId",
                table: "ChessGames",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "ChessGames",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "GameType",
                table: "ChessGames",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Result",
                table: "ChessGames",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "WhitePlayerId",
                table: "ChessGames",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ChessGames_BlackPlayerId",
                table: "ChessGames",
                column: "BlackPlayerId");

            migrationBuilder.CreateIndex(
                name: "IX_ChessGames_WhitePlayerId",
                table: "ChessGames",
                column: "WhitePlayerId");

            migrationBuilder.AddForeignKey(
                name: "FK_ChessGames_Users_BlackPlayerId",
                table: "ChessGames",
                column: "BlackPlayerId",
                principalTable: "Users",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ChessGames_Users_WhitePlayerId",
                table: "ChessGames",
                column: "WhitePlayerId",
                principalTable: "Users",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ChessGames_Users_BlackPlayerId",
                table: "ChessGames");

            migrationBuilder.DropForeignKey(
                name: "FK_ChessGames_Users_WhitePlayerId",
                table: "ChessGames");

            migrationBuilder.DropIndex(
                name: "IX_ChessGames_BlackPlayerId",
                table: "ChessGames");

            migrationBuilder.DropIndex(
                name: "IX_ChessGames_WhitePlayerId",
                table: "ChessGames");

            migrationBuilder.DropColumn(
                name: "BlackPlayerId",
                table: "ChessGames");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "ChessGames");

            migrationBuilder.DropColumn(
                name: "GameType",
                table: "ChessGames");

            migrationBuilder.DropColumn(
                name: "Result",
                table: "ChessGames");

            migrationBuilder.DropColumn(
                name: "WhitePlayerId",
                table: "ChessGames");
        }
    }
}
