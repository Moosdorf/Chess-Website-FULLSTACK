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
        Task<(int, Piece[][])> CreateGameAsync(int userId1, int userId2);
        Task<bool> MoveAsync(int chessId, string move);


        bool RemoveLastMove(int chessId);

        ChessGame EndGame(int chessId);


        IList<ChessGame> GetGames();

        Task<ChessGame?> GetGameAsync(int chessId);
    }
}
