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
        Task<(ChessGame, ChessInfo)> CreateGameAsync(string userName1, string userName2);
        Task<(ChessGame, ChessInfo)> CreateBotGameAsync(string userName1, bool white);

        Task<List<ChessGame>> GetMatchHistory(string username);
        Task<bool> MoveAsync(int chessId, string move, string FEN);

        public ChessModel CreateChessModel(ChessInfo chessState, ChessGame game, string sessionId);

        bool RemoveLastMove(int chessId);

        ChessGame EndGame(int chessId);


        IList<ChessGame> GetGames();

        Task<ChessGame?> GetGameAsync(int chessId);
    }
}
