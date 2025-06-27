using DataLayer.HelperMethods;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
public enum PieceType
{
    Empty,
    Pawn,
    Knight,
    Bishop,
    Rook,
    Queen,
    King
}

public abstract class Piece
{
    public Piece(bool white) 
    {
        IsWhite = white;
    }
    public PieceType Type { get; set; }
    public string Position { get; set; } = string.Empty;
    public int Moves { get; set; } = 0;
    public int Captures { get; set; } = 0;
    public bool IsAlive { get; set; } = true;
    public bool IsWhite { get; set; }
    public bool CanMove { get; set; } = false;
    public List<string> AvailableMoves { get; set; } = [];
    public List<string> AvailableCaptures { get; set; } = [];
    public List<string> Attackers { get; set; } = [];
    public List<string> Defenders { get; set; } = [];
    public abstract void FindMoves(Piece[][] board);
    public abstract bool Move();
    public abstract bool Capture();

    public void AddMove(Piece piece)
    {
        AvailableMoves.Add(piece.Position);
        if (piece.IsWhite == IsWhite) piece.Defenders.Add(Position);
        else  piece.Attackers.Add(Position);
    }

    public void AddCaptures(Piece piece)
    {
        AvailableCaptures.Add(piece.Position);
        piece.Attackers.Add(Position);
    }


    public bool UpdateMoves(Piece[][] board, int iRow, int iCol)
    {
        var piece = board[iRow][iCol];
        if (piece.Type == PieceType.Empty)
        {
            AddMove(piece);
            return true;
        }
        else if (piece.IsWhite != IsWhite)
        {
            AddCaptures(piece);
            if (piece.Type == PieceType.King)
            {
                King king = (King)piece;
                Console.WriteLine("hitting king with with " + this);
                king.Blockers = ChessMethods.FindCheckBlockers(board, (King)piece, this);
            }
        }
        return false; 
    }

    public override string? ToString()
    {
        return $"Type: {Type}, " +
               $"Position: {Position}, " +
               $"Moves: {Moves}, " +
               $"Captures: {Captures}, " +
               $"IsAlive: {IsAlive}, " +
               $"Color: {(IsWhite ? "White" : "Black")}";
    }
}
