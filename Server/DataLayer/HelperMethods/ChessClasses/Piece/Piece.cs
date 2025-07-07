using DataLayer.HelperMethods;
using System;
using System.Collections.Generic;
using System.IO.Pipelines;
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
    public PieceType Type { get; set; }
    public string Position { get; set; } = string.Empty;
    public bool IsWhite { get; set; }
    public bool Pinned { get; set; } = false;
    public List<string> PinnedSquares { get; set; } = [];
    public List<string> AvailableMoves { get; set; } = [];
    public List<string> AvailableCaptures { get; set; } = [];
    public List<string> Attackers { get; set; } = [];
    public List<string> Defenders { get; set; } = [];

    public Piece(bool white)
    {
        IsWhite = white;
    }

    public abstract void FindMoves(ChessInfo chessState);


    public void AddMove(ChessInfo chessState, Piece target)
    {
        if (Pinned)
        {
            Console.WriteLine("pinned - target " + target.Position);
            Console.WriteLine("pinned - PinnedSquares " + PinnedSquares.Count);
            if (!PinnedSquares.Contains(target.Position)) return;
        }

        if (chessState.InCheck && chessState.CheckedKing?.IsWhite == IsWhite && this != chessState.CheckedKing && !chessState.Blockers.Contains(target.Position))
        {
            return;
        }

        if (Type == PieceType.King)
        {
            if (target.Attackers.Count > 0)
            {
                return;
            }
        }



        if (Type != PieceType.Pawn)
        {
            if (target.Type == PieceType.Empty)
            {
                if ((chessState.Turn == "w") == IsWhite)
                {
                    target.Defenders.Add(Position);
                }
                else
                {
                    target.Attackers.Add(Position);
                }
            }
            else
            { // not empty square
                if (IsWhite != target.IsWhite) target.Attackers.Add(Position);
                else target.Defenders.Add(Position);
            }
        }

        AvailableMoves.Add(target.Position);
    }



    public void AddCaptures(ChessInfo chessState, Piece target)
    {

        if (Pinned)
        {
            Console.WriteLine("pinned");
            if (!PinnedSquares.Contains(target.Position)) return;
        }

        // if the target is the enemy king, find blocking squares and set states
        if (target.Type == PieceType.King && target.IsWhite != IsWhite)
        {
            King king = (King) target;
            ChessMethods.FindCheckBlockers(chessState, (King)target, this);
            chessState.InCheck = true;
            chessState.CheckedKing = king;
        }


        // if the attacker is the king, return if the enemy piece has a defender
        if (Type == PieceType.King)
        {
            if (target.Type != PieceType.Empty && target.Defenders.Count > 0)
            {
                return;
            }
        }

        // if the friendly king is in check, return if this square is not a blocker of the check
        if (chessState.InCheck && chessState.CheckedKing != this && chessState.CheckedKing?.IsWhite == IsWhite && !chessState.Blockers.Contains(target.Position)) 
        {
            return; 
        }

        // if the target is an empty square add this piece as a defender if it is the piece's turn else attacker
        if (target.Type == PieceType.Empty)
        {
            if ((chessState.Turn == "w") == IsWhite)
            {
                target.Defenders.Add(Position);
            } else
            {
                target.Attackers.Add(Position);
            }
        } else
        { // not empty square
            if (IsWhite != target.IsWhite) target.Attackers.Add(Position);
            else target.Defenders.Add(Position);
        }



        if (Type != PieceType.Pawn) // a pawn cannot capture in front
        {
            AvailableCaptures.Add(target.Position);
        } else
        {
            if (target.Type != PieceType.Empty && target.IsWhite != IsWhite) AvailableCaptures.Add(target.Position);
            if (target.Type == PieceType.Empty && target.Position == chessState.EnPassantSquare) AvailableCaptures.Add(target.Position);
        }
    }


    public bool UpdateMoves(ChessInfo chessState, int iRow, int iCol)
    {
        var piece = chessState.GameBoard[iRow][iCol];

        if (piece.Type == PieceType.Empty)
        {
            AddMove(chessState, piece);
            return true;
        }
        else if (piece.IsWhite != IsWhite)
        {
            AddCaptures(chessState, piece);
        }
        // else
        if (piece.IsWhite == IsWhite) piece.Defenders.Add(Position);
        return false; 
    }

    public override string? ToString()
    {
        return $"Type: {Type}, " +
               $"Position: {Position}, " +
               $"AvailableMoves: {AvailableMoves.Count}, " +
               $"AvailableCaptures: {AvailableCaptures.Count}, " +
               $"Color: {(IsWhite ? "White" : "Black")}";
    }
}
