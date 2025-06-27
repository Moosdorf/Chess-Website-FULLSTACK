using DataLayer.Entities.Chess;
using DataLayer.HelperMethods;
using DataLayer.Models.Chess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;


public class ChessInfo
{
    public Piece[][] GameBoard { get; private set; }
    public List<Piece> BlackPieces { get; private set; } = [];
    public List<Piece> WhitePieces { get; private set; } = [];
    public King BlackKing { get; private set; } = null!;
    public King WhiteKing { get; private set; } = null!;
    public int Moves { get; set; }
    

    public ChessInfo()
    {
        this.GameBoard = ChessMethods.CreateGameBoard();
        InitializeInfo();
    }
    public ChessInfo(List<Move> moves)
    {
        this.GameBoard = ChessMethods.CreateGameBoard();
        ReplayMoves(moves);
        InitializeInfo();
    }

    private void InitializeInfo()
    {
        foreach (Piece[] pieces in GameBoard)
        {
            foreach (Piece piece in pieces)
            {
                if (piece.Type == PieceType.Empty) continue;

                if (piece.IsWhite)
                {
                    WhitePieces.Add(piece);
                    if (piece.Type == PieceType.King)
                    {
                        WhiteKing = (King)piece;
                    }
                }
                else
                {
                    BlackPieces.Add(piece);
                    if (piece.Type == PieceType.King)
                    {
                        BlackKing = (King)piece;
                    }
                }
            }
        }
    }


    private void ReplayMoves(List<Move> moves)
    {
        // replay game to get to current state
        for (int i = 0; i < moves.Count; i++)
        {
            Console.WriteLine("Move: " + Moves); 
            ChessMethods.MakeMove(GameBoard, moves[i].MoveString);
            Moves++;
        }

        GameBoard = ChessMethods.findAvailableMoves(GameBoard);
    }

    public bool Move(string move)
    {
        // check if move is good
        var canMove = ChessMethods.ValidateMove(move, GameBoard);
        if (!canMove) return false;
        // make the move
        Console.WriteLine("Move: " + Moves);
        ChessMethods.MakeMove(GameBoard, move);
        GameBoard = ChessMethods.findAvailableMoves(GameBoard);
        Moves++;
        return true;
    }

    public override string? ToString()
    {
        var blackKingPos = BlackKing != null ? BlackKing.Position.ToString() : "null";
        var whiteKingPos = WhiteKing != null ? WhiteKing.Position.ToString() : "null";

        return $"Black Pieces: {BlackPieces?.Count ?? 0}\n" +
               $"White Pieces: {WhitePieces?.Count ?? 0}\n" +
               $"Black King Position: {blackKingPos}\n" +
               $"White King Position: {whiteKingPos}\n" +
               $"Moves: {Moves}";
    }
}
