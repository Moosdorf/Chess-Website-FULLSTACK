using DataLayer.Entities;
using DataLayer.Entities.Chess;
using DataLayer.Entities.Chess.Piece;
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
        Piece[][] CreateGame(int userId1, int userId2);
        Piece[][]? Move(Piece[][] chessBoard, (int, int) attacker, (int, int) victim);


        bool RemoveLastMove(int chessId);

        ChessGame EndGame(int chessId);


        IList<ChessGame> GetGames();

        ChessGame? GetGame(int chessId);
    }
}
