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
namespace DataLayer.DataServices;

public static class HelperMethods
{
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

    public static bool checkMove(PieceModel[][] chessBoard, (int, int) move, (int, int) from)
    {
        bool whitesTurn = chessBoard[from.Item1][from.Item2].color == "white";
        Console.WriteLine(whitesTurn);

        return false;
    } 

    public static void findAvailableMoves(PieceModel[][] chessBoard)
    {
        foreach (var row in chessBoard)
        {
            foreach (var piece in row)
            {
                if (piece.piece == "blank") continue;
                switch(piece.piece)
                {
                    case "pawn":
                        {
                            piece.availableMoves = findPawnMoves(chessBoard, piece);
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

    private static List<int[]> findPawnMoves(PieceModel[][] chessBoard, PieceModel piece)
    {
        
        throw new NotImplementedException();
    }
}
public class ChessDataService : IChessDataService
{
    private ChessContext _db;

    public ChessDataService(ChessContext context)
    {
        _db = context;
    }



    public PieceModel[][] CreateGame(int userId1, int userId2)
    {
        var chessBoard = new PieceModel[8][]; // ends as final result 

        var names = new string[]
        {
            "rook", "knight", "bishop", "queen", "king", "bishop", "knight", "rook"
        };

        for (int row = 0; row < 8; row++) // initiate each row in the jagged array (must be done otherwise we have no arrays to push to)
        {
            chessBoard[row] = new PieceModel[8];
        }

        for (int col = 0; col < 8; col++)
        {
            chessBoard[0][col] = new PieceModel { piece = names[col], color = "white", row = 0, col = col}; // insert white pieces
            chessBoard[7][col] = new PieceModel { piece = names[col], color = "black", row = 7, col = col}; // insert black pieces
        }


        // creating and pushing black pawns
        for (int i = 0; i < 8; i++)
        {
            chessBoard[1][i] = new PieceModel { piece = "pawn", color = "white", row = 1, col = i}; // insert white pawns
            chessBoard[6][i] = new PieceModel { piece = "pawn", color = "black", row = 6, col = i}; // insert black pawns

        }


        for (int row = 2; row < 6; row++)
        {
            for (int col = 0; col < 8; col++)
            {
                chessBoard[row][col] = new PieceModel { piece = "blank", color = "blank", row = row, col = col}; // insert white pawns
            }
        }
        
        return chessBoard;
    }
    public PieceModel[][]? Move(PieceModel[][] chessBoard, (int, int) attacker, (int, int) victim)
    {
        // row and cols
        int attackerRow = attacker.Item1;
        int attackerCol = attacker.Item2;
        int victimRow = victim.Item1;
        int victimCol = victim.Item2;

        PieceModel attackerPiece = chessBoard[attackerRow][attackerCol]; // attacker
        PieceModel victimPiece = chessBoard[victimRow][victimCol]; // defender

        string chessNotation = HelperMethods.convertToChessNotation(attackerRow, attackerCol, victimRow, victimCol, attackerPiece.piece[0], victimPiece.piece != "blank");

        bool canMove = HelperMethods.checkMove(chessBoard, victim, attacker);   

        if (true) // do the moving
        {


            chessBoard[victimRow][victimCol] = chessBoard[attackerRow][attackerCol]; // "move" the attacker
            chessBoard[attackerRow][attackerCol] = new PieceModel { piece = "blank", color = "blank" }; // kill the defender

            attackerPiece.moves += 1;
            attackerPiece.row = victimRow;
            attackerPiece.col = victimCol;



            return chessBoard; // return new state
        }
        // return null;

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

