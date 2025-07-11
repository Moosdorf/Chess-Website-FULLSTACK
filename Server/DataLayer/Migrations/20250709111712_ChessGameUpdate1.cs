using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataLayer.Migrations
{
    /// <inheritdoc />
    public partial class ChessGameUpdate1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ChessGames_Users_BlackPlayerId",
                table: "ChessGames");

            migrationBuilder.DropForeignKey(
                name: "FK_ChessGames_Users_WhitePlayerId",
                table: "ChessGames");

            migrationBuilder.AlterColumn<int>(
                name: "WhitePlayerId",
                table: "ChessGames",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "BlackPlayerId",
                table: "ChessGames",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_ChessGames_Users_BlackPlayerId",
                table: "ChessGames",
                column: "BlackPlayerId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ChessGames_Users_WhitePlayerId",
                table: "ChessGames",
                column: "WhitePlayerId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
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

            migrationBuilder.AlterColumn<int>(
                name: "WhitePlayerId",
                table: "ChessGames",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<int>(
                name: "BlackPlayerId",
                table: "ChessGames",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

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
    }
}
