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
        Task<(int, Piece[][])> CreateGameAsync(int userId1, int userId2);
        Task<bool> MoveAsync(int chessId, string move);

        public ChessModel CreateChessModel(ChessInfo chessState, ChessGame game);

        bool RemoveLastMove(int chessId);

        ChessGame EndGame(int chessId);


        IList<ChessGame> GetGames();

        Task<ChessGame?> GetGameAsync(int chessId);
    }
}
