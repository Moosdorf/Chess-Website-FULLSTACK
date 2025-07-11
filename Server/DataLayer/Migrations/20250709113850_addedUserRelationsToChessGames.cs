using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataLayer.Migrations
{
    /// <inheritdoc />
    public partial class addedUserRelationsToChessGames : Migration
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

            migrationBuilder.RenameColumn(
                name: "WhitePlayerId",
                table: "ChessGames",
                newName: "WhiteId");

            migrationBuilder.RenameColumn(
                name: "BlackPlayerId",
                table: "ChessGames",
                newName: "BlackId");

            migrationBuilder.RenameIndex(
                name: "IX_ChessGames_WhitePlayerId",
                table: "ChessGames",
                newName: "IX_ChessGames_WhiteId");

            migrationBuilder.RenameIndex(
                name: "IX_ChessGames_BlackPlayerId",
                table: "ChessGames",
                newName: "IX_ChessGames_BlackId");

            migrationBuilder.AddForeignKey(
                name: "FK_ChessGames_Users_BlackId",
                table: "ChessGames",
                column: "BlackId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ChessGames_Users_WhiteId",
                table: "ChessGames",
                column: "WhiteId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ChessGames_Users_BlackId",
                table: "ChessGames");

            migrationBuilder.DropForeignKey(
                name: "FK_ChessGames_Users_WhiteId",
                table: "ChessGames");

            migrationBuilder.RenameColumn(
                name: "WhiteId",
                table: "ChessGames",
                newName: "WhitePlayerId");

            migrationBuilder.RenameColumn(
                name: "BlackId",
                table: "ChessGames",
                newName: "BlackPlayerId");

            migrationBuilder.RenameIndex(
                name: "IX_ChessGames_WhiteId",
                table: "ChessGames",
                newName: "IX_ChessGames_WhitePlayerId");

            migrationBuilder.RenameIndex(
                name: "IX_ChessGames_BlackId",
                table: "ChessGames",
                newName: "IX_ChessGames_BlackPlayerId");

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
    }
}
