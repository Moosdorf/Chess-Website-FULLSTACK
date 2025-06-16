using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataLayer.Migrations
{
    /// <inheritdoc />
    public partial class removed_users_from_chessgame : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ChessGames_Users_User1Id",
                table: "ChessGames");

            migrationBuilder.DropForeignKey(
                name: "FK_ChessGames_Users_User2Id",
                table: "ChessGames");

            migrationBuilder.DropIndex(
                name: "IX_ChessGames_User1Id",
                table: "ChessGames");

            migrationBuilder.DropIndex(
                name: "IX_ChessGames_User2Id",
                table: "ChessGames");

            migrationBuilder.DropColumn(
                name: "User1Id",
                table: "ChessGames");

            migrationBuilder.DropColumn(
                name: "User2Id",
                table: "ChessGames");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "User1Id",
                table: "ChessGames",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "User2Id",
                table: "ChessGames",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_ChessGames_User1Id",
                table: "ChessGames",
                column: "User1Id");

            migrationBuilder.CreateIndex(
                name: "IX_ChessGames_User2Id",
                table: "ChessGames",
                column: "User2Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ChessGames_Users_User1Id",
                table: "ChessGames",
                column: "User1Id",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ChessGames_Users_User2Id",
                table: "ChessGames",
                column: "User2Id",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
