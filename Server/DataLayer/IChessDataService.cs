using ChessServer.Models;
using DataLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer
{
    public interface IChessDataService
    {
        PieceModel[][] CreateGame(int userId1, int userId2);
        PieceModel[][]? Move(PieceModel[][] chessBoard, (int, int) attacker, (int, int) victim);

        ChessMove CreateMove(string move, int chessId);

        bool RemoveLastMove(int chessId);

        ChessGame EndGame(int chessId);

        IList<UserChessGames> GetGames(int userId);

        IList<ChessGame> GetGames();

        IList<ChessMove> GetMoves(int chessId);
        ChessGame? GetGame(int chessId);
    }
}
