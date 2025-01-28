using DataLayer.Entities;
using System;
using System.Collections.Generic;
using System.IO.Pipelines;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;
using ChessServer.Models;
namespace DataLayer;

public class ChessDataService : IChessDataService
{
    private ChessContext db;

    public ChessDataService()
    {
        db = new ChessContext();
    }

    public PieceModel[][] CreateGame(int userId1, int userId2)
    {
        var chessBoard = new PieceModel[8][]; // ends as final result 

        var names = new string[]
        {
            "rook", "knight", "bishop", "queen", "king", "bishop", "knight", "rook"
        };

        for (int row = 0; row < 8; row++)
        {
            chessBoard[row] = new PieceModel[8];
        }

        for (int col = 0; col < 8; col++)
        {
            chessBoard[0][col] = new PieceModel { piece = names[col], color = "white"}; // insert white pieces
            chessBoard[7][col] = new PieceModel { piece = names[col], color = "black"}; // insert black pieces
        }


        // creating and pushing black pawns
        for (int i = 0; i < 8; i++)
        {
            chessBoard[1][i] = new PieceModel { piece = "pawn", color = "white"}; // insert white pawns
            chessBoard[6][i] = new PieceModel { piece = "pawn", color = "black"}; // insert black pawns

        }


        for (int row = 2; row < 6; row++)
        {
            for (int col = 0; col < 8; col++)
            {
                chessBoard[row][col] = new PieceModel { piece = "blank", color = "blank"}; // insert white pawns

            }
        }

        return chessBoard;
    }

    public ChessMove CreateMove(string move, int chessId)
    {
        throw new NotImplementedException();
    }

    public ChessGame EndGame(int chessId)
    {
        throw new NotImplementedException();
    }

    public ChessGame? GetGame(int chessId)
    {
        throw new NotImplementedException();
    }

    public IList<UserChessGames> GetGames(int userId)
    {
        return db.Users.Where(x => x.UserId == userId).Select(x => x.ChessGames).First();
    }

    public IList<ChessGame> GetGames()
    {
        throw new NotImplementedException();
    }

    public IList<ChessMove> GetMoves(int chessId)
    {
        throw new NotImplementedException();
    }

    public bool RemoveLastMove(int chessId)
    {
        throw new NotImplementedException();
    }
}
