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
    public Piece(bool white) 
    {
        IsWhite = white;
    }
    public PieceType Type { get; set; }
    public string Position { get; set; } = string.Empty;
    public bool Pinned { get; set; } = false;
    public bool IsWhite { get; set; }
    public List<string> AvailableMoves { get; set; } = [];
    public List<string> AvailableCaptures { get; set; } = [];
    public List<string> Attackers { get; set; } = [];
    public List<string> Defenders { get; set; } = [];
    public abstract void FindMoves(ChessInfo chessState);


    public void AddMove(ChessInfo chessState, Piece target)
    {

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
            if (IsWhite == (chessState.Turn != "w")) target.Attackers.Add(Position); 
            else target.Defenders.Add(Position); 
        }


        AvailableMoves.Add(target.Position);
    }



    public void AddCaptures(ChessInfo chessState, Piece target)
    {
        if (target.Type == PieceType.King)
        {
            King king = (King) target;
            ChessMethods.FindCheckBlockers(chessState, (King)target, this);
            chessState.InCheck = true;
            chessState.CheckedKing = king;
        }


        if (chessState.InCheck && chessState.CheckedKing?.IsWhite == IsWhite && !chessState.Blockers.Contains(target.Position)) 
        {
            return; 
        }

        if (IsWhite != (chessState.Turn == "w")) target.Attackers.Add(Position);
        else target.Defenders.Add(Position);

        if (Type != PieceType.Pawn)
        {
            AvailableCaptures.Add(target.Position);
        } else
        {
            if (target.Type != PieceType.Empty && target.IsWhite != IsWhite) AvailableCaptures.Add(target.Position);
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
