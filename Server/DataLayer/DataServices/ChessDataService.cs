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



    public async Task<(ChessGame, ChessInfo)> CreateGameAsync(int userId1, int userId2)
    {
        var chessBoard = new ChessInfo();
        var dbEntryChessGame = new ChessGame();
        _db.ChessGames.Add(dbEntryChessGame);
        await _db.SaveChangesAsync();
        return (dbEntryChessGame, chessBoard);
    }

    public async Task<bool> MoveAsync(int chessId, string move, string FEN)
    {
        var newMove = new Move()
        {
            ChessGameId = chessId,
            MoveString = move,
            FEN = FEN
        };

        _db.Moves.Add(newMove);
        var result = await _db.SaveChangesAsync() > 0;
        return result;
    }

    public ChessModel CreateChessModel(ChessInfo chessState, ChessGame game) 
    {
        var isWhite = chessState.Turn == "w";
        var king = (isWhite) ? chessState.WhiteKing : chessState.BlackKing;
        var inCheck = chessState.InCheck;
        var blockers = chessState.Blockers;

        bool gameDone = false;
        if (inCheck)
        {
            var pieces = (isWhite) ? chessState.WhitePieces : chessState.BlackPieces;
            gameDone = !pieces.Any(x => x.AvailableMoves.Count > 0);
        }


        return new ChessModel
        { Chessboard = chessState.GameBoard, FEN = ChessMethods.GenerateFEN(chessState), Id = game.Id, IsWhite = isWhite, Check = inCheck, CheckMate = gameDone, BlockCheckPositions = blockers};
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

