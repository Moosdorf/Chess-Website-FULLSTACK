using DataLayer.Entities;
using System;
using System.Collections.Generic;
using System.IO.Pipelines;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;
using DataLayer.Entities.Chess;
using DataLayer.Models.Chess;
using DataLayer.Entities.Chess.Piece;
namespace DataLayer.DataServices;

public static class HelperMethods
{
    public static string RowColToRankFile(int row,
                                          int col)
    {
        char file = (char)(col + 97); // converting integer to char, just add 97 to find alphabet. 0 + 97 = 'a', 1 + 97 = 'b' and so on.
        int rank = row + 1; // create rank numbers (just increase by one)

        return $"{file}{rank}";
    }

    public static string convertToChessNotation(int fRow,
                                          int fCol,
                                          int tRow,
                                          int tCol,
                                          char piece,
                                          bool kill = false,
                                          bool castleShort = false,
                                          bool castleLong = false,
                                          bool check = false,
                                          bool promotion = false,
                                          char promotionP = 'X')
    {
        if (castleShort) { return "O-O"; }
        if (castleLong) { return "O-O-O"; }
  
        // t = to, f = from
        int tRank = tRow + 1; // create rank numbers (just increase by one)
        int fRank = fRow + 1;
        char tFile = (char)(tCol + 97); // converting integer to char, just add 97 to find alphabet. 0 + 97 = 'a', 1 + 97 = 'b' and so on.
        char fFile = (char)(fCol + 97);

        char? killSymbol = kill ? 'x' : null;

        if (promotion)
        {
            return $"{fFile}{tRank}={promotionP}";
        }

        if (piece == 'p')
        {
            if (kill) return $"{fFile}{killSymbol}{tFile}{tRank}";
            return $"{fFile}{tRank}";
        }

        return $"{char.ToUpper(piece)}{fFile}{fRank}{killSymbol}{tFile}{tRank}";
    }

    public static bool checkMove(Piece[][] chessBoard, (int, int) move, (int, int) from)
    {
        bool whitesTurn = chessBoard[from.Item1][from.Item2].IsWhite;
        Console.WriteLine(whitesTurn);

        return false;
    } 

    public static void findAvailableMoves(Piece[][] chessBoard)
    {
        foreach (var row in chessBoard)
        {
            foreach (var piece in row)
            {
                if (piece.Type == "empty") continue;
                switch(piece.Type)
                {
                    case "pawn":
                        {
                            piece.FindMoves();
                            break;
                        }
                    case "rook":
                        {
                            break;
                        }
                    case "bishop":
                        {
                            break;
                        }
                    case "knight":
                        {
                            break;
                        }
                    case "king":
                        {
                            break;
                        }
                    case "queen":
                        {
                            break;
                        }
                }
            }
        }

    }
}
public class ChessDataService : IChessDataService
{
    private ChessContext _db;

    public ChessDataService(ChessContext context)
    {
        _db = context;
    }



    public Piece[][] CreateGame(int userId1, int userId2)
    {
        var chessBoard = new Piece[8][]; // ends as final result 

        for (int row = 0; row < 8; row++) // initiate each row in the jagged array (must be done otherwise we have no arrays to push to)
        {
            chessBoard[row] = new Piece[8];
        }

        chessBoard[0][0] = new Rook(true) { Type = "rook", Position = HelperMethods.RowColToRankFile(0, 0) };
        chessBoard[0][1] = new Knight(true) { Type = "knight", Position = HelperMethods.RowColToRankFile(0, 1) };
        chessBoard[0][2] = new Bishop(true) { Type = "bishop", Position = HelperMethods.RowColToRankFile(0, 2) };
        chessBoard[0][3] = new Queen(true) { Type = "queen", Position = HelperMethods.RowColToRankFile(0, 3) };
        chessBoard[0][4] = new King(true) { Type = "king", Position = HelperMethods.RowColToRankFile(0, 4) };
        chessBoard[0][5] = new Bishop(true) { Type = "bishop", Position = HelperMethods.RowColToRankFile(0, 5) };
        chessBoard[0][6] = new Knight(true) { Type = "knight", Position = HelperMethods.RowColToRankFile(0, 6) };
        chessBoard[0][7] = new Rook(true) { Type = "rook", Position = HelperMethods.RowColToRankFile(0, 7) };

        chessBoard[7][0] = new Rook(false) { Type = "rook", Position = HelperMethods.RowColToRankFile(7, 0) };
        chessBoard[7][1] = new Knight(false) { Type = "knight", Position = HelperMethods.RowColToRankFile(7, 1) };
        chessBoard[7][2] = new Bishop(false) { Type = "bishop", Position = HelperMethods.RowColToRankFile(7, 2) };
        chessBoard[7][3] = new Queen(false) { Type = "queen", Position = HelperMethods.RowColToRankFile(7, 3) };
        chessBoard[7][4] = new King(false) { Type = "king", Position = HelperMethods.RowColToRankFile(7, 4) };
        chessBoard[7][5] = new Bishop(false) { Type = "bishop", Position = HelperMethods.RowColToRankFile(7, 5) };
        chessBoard[7][6] = new Knight(false) { Type = "knight", Position = HelperMethods.RowColToRankFile(7, 6) };
        chessBoard[7][7] = new Rook(false) { Type = "rook", Position = HelperMethods.RowColToRankFile(7, 7) };



        // creating and pushing black pawns
        for (int col = 0; col < 8; col++)
        {
            chessBoard[1][col] = new Pawn(true) { Type = "pawn", Position = HelperMethods.RowColToRankFile(1, col) }; // insert white pawns
            chessBoard[6][col] = new Pawn(false) { Type = "pawn", Position = HelperMethods.RowColToRankFile(6, col) }; // insert black pawns

        }


        for (int row = 2; row < 6; row++)
        {
            for (int col = 0; col < 8; col++)
            {
                chessBoard[row][col] = new Empty(false) { Type = "empty", Position = HelperMethods.RowColToRankFile(row, col) };
            }
        }
        
        return chessBoard;
    }
    public Piece[][]? Move(Piece[][] chessBoard, (int, int) attacker, (int, int) victim)
    {
        
        return null;

    }


    public ChessGame EndGame(int chessId)
    {
        throw new NotImplementedException();
    }

    public ChessGame? GetGame(int chessId)
    {
        throw new NotImplementedException();
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

