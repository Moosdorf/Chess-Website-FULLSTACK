using DataLayer.Entities;
using DataLayer.Entities.Chess;
using DataLayer.Models.Chess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.DataServices
{
    public interface IChessDataService
    {
        PieceModel[][] CreateGame(int userId1, int userId2);
        PieceModel[][]? Move(PieceModel[][] chessBoard, (int, int) attacker, (int, int) victim);


        bool RemoveLastMove(int chessId);

        ChessGame EndGame(int chessId);


        IList<ChessGame> GetGames();

        ChessGame? GetGame(int chessId);
    }
}
