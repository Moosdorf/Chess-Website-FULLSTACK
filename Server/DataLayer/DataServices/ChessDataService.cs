using DataLayer.Entities;
using DataLayer.Entities.Chess;
using DataLayer.HelperMethods;
using DataLayer.Models.Chess;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO.Pipelines;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;
namespace DataLayer.DataServices;

public class ChessDataService : IChessDataService
{
    private ChessContext _db;
    private IDataService _dataService;

    public ChessDataService(ChessContext context, IDataService dataService)
    {
        _db = context;
        _dataService = dataService;
    }



    public async Task<(ChessGame, ChessInfo)> CreateGameAsync(string userName1, string userName2)
    {
        var player1 = await _dataService.GetUser(userName1);
        var player2 = await _dataService.GetUser(userName2);

       if (player1 == null && player2 == null)
       {
            Console.WriteLine("players null");
            return (null, null);
       }


        var chessBoard = new ChessInfo();
        var dbEntryChessGame = new ChessGame()
        {
            WhitePlayer = player1,
            BlackPlayer = player2,
            GameType = GameType.Multiplayer
        };

        _db.ChessGames.Add(dbEntryChessGame);
        await _db.SaveChangesAsync();
        return (dbEntryChessGame, chessBoard);
    }

    public async Task<(ChessGame, ChessInfo)> CreateBotGameAsync(string userName1, bool white)
    {
        var player1 = await _dataService.GetUser(userName1);
        var botPlayer = await _dataService.GetUser("stockfish");
        // if (player1 == null) return (null, null);

        var chessBoard = new ChessInfo();
        var dbEntryChessGame = new ChessGame()
        {
            WhiteId = (!white) ? player1.Id : botPlayer.Id,
            WhitePlayer = (white) ? player1 : botPlayer,
            BlackId = (!white) ? player1.Id : botPlayer.Id,
            BlackPlayer = (!white) ? player1 : botPlayer,

            GameType = GameType.Bot
        };

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

    public ChessModel CreateChessModel(ChessInfo chessState, ChessGame game, string sessionId) 
    {
        var isWhite = chessState.Turn == "w";
        var king = (isWhite) ? chessState.WhiteKing : chessState.BlackKing;
        var inCheck = chessState.InCheck;
        var blockers = chessState.Blockers;

        bool gameDone = false;

        var pieces = (isWhite) ? chessState.WhitePieces : chessState.BlackPieces;
        // if game is done
        gameDone = !pieces.Any(x => x.AvailableMoves.Count > 0 || x.AvailableCaptures.Count > 0);

        if (inCheck)
        {
            pieces.ForEach(x => Console.WriteLine(x));
        }
        if (gameDone && inCheck)
        {
            // if game done and someone lost
        }

        var currentPlayer = (isWhite) ? game.WhitePlayer.Username : game.BlackPlayer.Username;

        return new ChessModel
            { SessionId = sessionId, CurrentPlayer = currentPlayer, Players = [game.WhitePlayer.Username, game.BlackPlayer.Username] , LastMove = chessState.LastMove, Chessboard = chessState.GameBoard, FEN = ChessMethods.GenerateFEN(chessState), Id = game.Id, IsWhite = isWhite, Check = inCheck, CheckMate = gameDone, BlockCheckPositions = blockers };
    }


    public ChessGame EndGame(int chessId)
    {
        throw new NotImplementedException();
    }

    public async Task<ChessGame?> GetGameAsync(int chessId)
    {
        var game = await _db.ChessGames
                        .Include(g => g.Moves)
                        .Include(g => g.BlackPlayer)
                        .Include(g => g.WhitePlayer)
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

