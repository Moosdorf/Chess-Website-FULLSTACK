using DataLayer.Entities;
using DataLayer.Entities.Chess;
using DataLayer.HelperMethods;
using DataLayer.Models.Chess;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO.Pipelines;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;
namespace DataLayer.DataServices;

public class ChessDataService : IChessDataService
{
    private ChessContext _db;

    public ChessDataService(ChessContext context)
    {
        _db = context;
    }



    public async Task<(int, Piece[][])> CreateGameAsync(int userId1, int userId2)
    {
        var chessBoard = ChessMethods.CreateGameBoard();
        var dbEntryChessGame = new ChessGame();
        _db.ChessGames.Add(dbEntryChessGame);
        await _db.SaveChangesAsync();
        return (dbEntryChessGame.Id, chessBoard);
    }

    public async Task<bool> MoveAsync(int chessId, string move)
    {
        var newMove = new Move()
        {
            ChessGameId = chessId,
            MoveString = move
        };

        _db.Moves.Add(newMove);
        var result = await _db.SaveChangesAsync() > 0;
        return result;
    }

    public ChessModel CreateChessModel(ChessInfo chessState, ChessGame game) 
    {
        var moves = (chessState.Moves);
        var isWhite = (moves % 2) == 0;
        var king = (isWhite) ? chessState.WhiteKing : chessState.BlackKing;
        var inCheck = king.Check;
        var blockers = king.Blockers;
        return new ChessModel
        { Chessboard = chessState.GameBoard, Id = game.Id, IsWhite = isWhite, Check = inCheck, BlockCheckPositions = blockers, Moves = moves };
    }


    public ChessGame EndGame(int chessId)
    {
        throw new NotImplementedException();
    }

    public async Task<ChessGame?> GetGameAsync(int chessId)
    {
        var game = await _db.ChessGames
                        .Include(g => g.Moves)
                        .FirstOrDefaultAsync(g => g.Id == chessId);
        return game;
    }

    public IList<ChessGame> GetGames()
    {
        throw new NotImplementedException();
    }


    public bool RemoveLastMove(int chessId)
    {
        throw new NotImplementedException();
    }
}

